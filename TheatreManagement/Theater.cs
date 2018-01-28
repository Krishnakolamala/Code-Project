using System.Collections.Generic;

namespace TheatreManagement
{
  /// <summary>
  /// Theater Seats
  /// </summary>
  class RowSeating
    {
        public int Row { get; set; }
        public List<Section> SectionData { get; set; }
    }

    class Section
    {
        public int SecetionNo { get; set; }
        public int NoofSeats { get; set; }
    }
}
