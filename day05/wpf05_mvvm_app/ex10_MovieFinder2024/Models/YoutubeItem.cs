using System.Windows.Media.Imaging;

namespace ex10_MovieFinder2024.Models
{
    public class YoutubeItem
    {
        public string Title {  get; set; }
        public string Author {  get; set; }
        public string ChannelTitle {  get; set; }
        public string URL{ get; set; }
        public BitmapImage Thumbnail { get; set; }

        public DateTime? Reg_Date { get; set; } //최초에는 없기때문에 Nullable 지정

        //쿼리 파트
        
    }
}
