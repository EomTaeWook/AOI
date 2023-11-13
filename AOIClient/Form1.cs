using AOIClient.Internal;
using AOIClient.Modules;
using AOIClient.Net;
using Dignus.Coroutine;
using Protocol.CAndS;
using Share;
using System.Collections;

namespace AOIClient
{
    public partial class Form1 : Form
    {
        //protected override CreateParmas CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle != 0x02000000;
        //        return cp;
        //    }
        //}
        private CoroutineHandler _coroutineHandler = new CoroutineHandler();
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
            _coroutineHandler.Start(UpdateUI());
            var task = Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(33);
                    _coroutineHandler.UpdateCoroutines(33);
                }
            });

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Init();
            this.KeyPreview = true;
            this.Paint += Form1_Paint;
            this.btnAdd.Click += BtnAdd_Click;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            ClientModule.Instance.AddNpc();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Pen blackPen = new Pen(Color.Black, 1);

            var maxLength = GameManager.Instance.Map.GetMaxX() * GameManager.CellSize;

            //°¡·Î
            for (int i = 0; i <= GameManager.Instance.Map.GetMaxX(); ++i)
            {
                e.Graphics.DrawLine(blackPen, 0, GameManager.CellSize * i, maxLength, GameManager.CellSize * i);
            }
            for (int i = 0; i <= GameManager.Instance.Map.GetMaxY(); ++i)
            {
                e.Graphics.DrawLine(blackPen, GameManager.CellSize * i, 0, GameManager.CellSize * i, maxLength);
            }
            PlayerPaint(e.Graphics);
            FogPaint(e.Graphics);
            OtherPlayerPaint(e.Graphics);
        }
        public IEnumerator UpdateUI()
        {
            while (true)
            {
                this.BeginInvoke(RefreshUI);
                yield return null;
            }
        }
        public void RefreshUI()
        {
            this.Invalidate();
            this.Update();
        }
        private void OtherPlayerPaint(Graphics graphics)
        {
            if (GameManager.Instance.UserPlayer == null)
            {
                return;
            }

            foreach (var item in GameManager.Instance.Players.Values)
            {
                graphics.FillEllipse(new SolidBrush(Color.Green),
                new Rectangle(item.CellPos.X * GameManager.CellSize,
                item.CellPos.Y * GameManager.CellSize,
                GameManager.CellSize,
                GameManager.CellSize));

                graphics.DrawString(item.Nickname,
                new Font("Arial", 10),
                new SolidBrush(Color.Black),
                new PointF(item.CellPos.X * GameManager.CellSize,
                item.CellPos.Y * GameManager.CellSize));
            }
        }
        private void PlayerPaint(Graphics graphics)
        {
            if (GameManager.Instance.UserPlayer == null)
            {
                return;
            }

            graphics.FillEllipse(new SolidBrush(Color.Red),
                new Rectangle(
                    GameManager.Instance.UserPlayer.CellPos.X * GameManager.CellSize,
                    GameManager.Instance.UserPlayer.CellPos.Y * GameManager.CellSize,
                    GameManager.CellSize,
                    GameManager.CellSize));

            graphics.DrawString(GameManager.Instance.UserPlayer.Nickname,
                new Font("Arial", 10),
                new SolidBrush(Color.Black),
                new PointF(GameManager.Instance.UserPlayer.CellPos.X * GameManager.CellSize, GameManager.Instance.UserPlayer.CellPos.Y * GameManager.CellSize));
        }
        private void FogPaint(Graphics graphics)
        {
            if (GameManager.Instance.UserPlayer == null)
            {
                return;
            }
            var characterCellIndex = GameManager.Instance.Map.GetCellIndex(GameManager.Instance.UserPlayer.CellPos);

            var indexes = new List<int>(GameManager.Instance.Map.GetAroundCellByIndex(characterCellIndex, 1));

            for (int x = 0; x < GameManager.Instance.Map.GetMaxX(); ++x)
            {
                for (int y = 0; y < GameManager.Instance.Map.GetMaxY(); ++y)
                {
                    var vector = new Vector2Int(x, y);

                    var index = GameManager.Instance.Map.GetCellIndex(vector);

                    if (indexes.Contains(index) == true)
                    {
                        continue;
                    }

                    graphics.FillRectangle(new SolidBrush(Color.FromArgb(125, Color.Black)),
                    new Rectangle(
                        x * GameManager.CellSize,
                        y * GameManager.CellSize,
                        GameManager.CellSize,
                        GameManager.CellSize));
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Left)
            {
                ClientModule.Instance.Send(Packet.MakePacket(CSProtocol.Move,
                    new Move()
                    {
                        X = -1,
                        Y = 0
                    }));
            }
            else if (keyData == Keys.Right)
            {
                ClientModule.Instance.Send(Packet.MakePacket(CSProtocol.Move,
                    new Move()
                    {
                        X = 1,
                        Y = 0
                    }));
            }
            else if (keyData == Keys.Down)
            {
                ClientModule.Instance.Send(Packet.MakePacket(CSProtocol.Move,
                    new Move()
                    {
                        X = 0,
                        Y = 1
                    }));
            }
            else if (keyData == Keys.Up)
            {
                ClientModule.Instance.Send(Packet.MakePacket(CSProtocol.Move,
                    new Move()
                    {
                        X = 0,
                        Y = -1
                    }));
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Init()
        {
            ClientModule.Instance.Connect();
        }

    }
}