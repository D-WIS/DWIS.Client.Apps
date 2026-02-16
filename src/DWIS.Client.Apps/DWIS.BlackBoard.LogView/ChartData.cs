namespace DWIS.BlackBoard.LogView
{
    public class AppControl
    {
        public bool Play { get; set; } = true;
        public bool FullScreen { get; set; }
        public TimeSpan DisplaySpan { get; set; } = TimeSpan.MaxValue;
    }


    public class ChartData
    {
        public string Prototype { get; set; }        
        public double ConversionFactor { get; set; }
        public string AxisName { get; set; }
    }

    public class AxisData
    {
        public string Name { get; set; }
        public string Label { get; set; }
    }
}
