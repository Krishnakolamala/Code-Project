using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
