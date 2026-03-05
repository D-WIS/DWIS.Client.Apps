using DWIS.API.DTO;

namespace DWIS.BlackBoard.LogView
{
    public class AppControl
    {
        public bool Play { get; set; } = true;
        public bool FullScreen { get; set; }
        public TimeSpan DisplaySpan { get; set; } = TimeSpan.MaxValue;
        public bool ShowInspection { get; set; }


        public List<NodeIdentifier> ExludedCharts { get; private set; } = new List<NodeIdentifier>();

    }
}
