using System;
using System.Threading.Tasks;
using GGM.Application.Attribute;
using GGM.Application.Service;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Protocol;

namespace GGM.TCPService
{
    using Router;

    public abstract class TCPService<TSession, TPacketInfo> : IService
        where TSession : AppSession<TSession, TPacketInfo>, new()
        where TPacketInfo : PacketInfo
    {
        private readonly IReceiveFilterFactory<TPacketInfo> _receiveFilterFactory;

        private readonly IRouter _router = new DefaultRouter();
        private IAppServer<TSession, TPacketInfo> _server;

        public TCPService(IReceiveFilterFactory<TPacketInfo> receiveFilterFactory, params object[] controllers)
        {
            if (controllers != null)
                foreach (var controller in controllers)
                    _router.RegisterController(controller);

            if (receiveFilterFactory == null)
                throw new ArgumentNullException(nameof(receiveFilterFactory));

            _receiveFilterFactory = receiveFilterFactory;
        }

        #region Config
        
        [Config("TCPService.IP")]
        public string IP { get; set; }

        [Config("TCPService.Port")]
        public int Port { get; set; }

        [Config("TCPService.Name")]
        public string Name { get; set; }

        [Config("TCPService.MaxConnectionNumber")]
        public int MaxConnectionNumber { get; set; } = 100;
        #endregion Config

        public IServerConfig ServerConfig { get; private set; }

        public Guid ID { get; set; }

        public virtual Task Boot(string[] arguments)
        {
            // Config 적용
            ServerConfig = new ServerConfig
            {
                Name = Name,
                Ip = IP,
                Port = Port,
                MaxConnectionNumber = MaxConnectionNumber,
                Mode = SocketMode.Tcp
            };

            // 서버 생성과 이벤트 연결
            _server = new TCPServer(_receiveFilterFactory);
            _server.NewSessionConnected += OnConnected;
            _server.SessionClosed += OnDisconnected;
            _server.NewRequestReceived += OnReceived;

            // 서버 설정
            var server = _server as TCPServer;
            var isFailedConfigure = !server.Setup(ServerConfig);
            if (isFailedConfigure)
                throw new Exception("Fail to setup.");

            // 서버 시작
            if (server.Start())
                return server.StopTask;
            throw new Exception("Fail to start.");
        }

        protected abstract void OnConnected(TSession session);
        protected abstract void OnDisconnected(TSession session, CloseReason closeReason);

        // Warring : 비동기 메소드를 void 반환형으로 사용하는 것은 바람직 하지 못하나, 여기선 이렇게 씀.
        private async void OnReceived(TSession session, TPacketInfo packetInfo)
        {
            await _router.Route(packetInfo, session as AppSession);
        }

        private class TCPServer : AppServer<TSession, TPacketInfo>
        {
            private readonly TaskCompletionSource<bool> _stopTaskCompletionSource = new TaskCompletionSource<bool>();

            public TCPServer(IReceiveFilterFactory<TPacketInfo> receiveFilterFactory) : base(receiveFilterFactory)
            {
            }

            public Task StopTask => _stopTaskCompletionSource.Task;

            public bool Setup(IServerConfig config)
            {
                if (config == null)
                    throw new ArgumentNullException(nameof(config));
                return Setup(new RootConfig(), config, null);
            }

            protected override void OnStopped()
            {
                base.OnStopped();
                _stopTaskCompletionSource.SetResult(true);
            }
        }
    }
}