using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartingCSharp.Model
{
  public class Ship
  {
    public int MMSI { get; set; }
    public string ShipName { get; set; }
    public string Location { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
  }
}
