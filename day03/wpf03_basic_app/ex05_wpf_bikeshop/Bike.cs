using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ex05_wpf_bikeshop
{
    //NotiFier를 상송받으면 AutoProperty {get; set;}은 사용할수 없음
    public class Bike : NotiFier
    {
        private double speed;
        private Color color;
        public double Speed {
            get { return speed; } 
            set {  speed = value;

                OnPropertyChanged(nameof(Speed));
            } 
        } 
        public Color Color {
            get {return color; }
            set { color = value;
                OnPropertyChanged(nameof(color));
            }
        }
    }
}
