using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using toyproject.Models;


namespace toyproject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitComboDateFormDB();
        }

        private void InitComboDateFormDB()
        {
            using (SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(Models.GimhaeFood.Category_QUERY, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet dSet = new DataSet();
                adapter.Fill(dSet);
                List<String> saveCategory = new List<String>();

                foreach (DataRow row in dSet.Tables[0].Rows)
                {
                    saveCategory.Add(Convert.ToString(row["Category"]));
                }

                CboReqDate.ItemsSource = saveCategory;

            }
        }
        private async void BtnReqRealtime_Click(object sender, RoutedEventArgs e)
        {
            // 페이지 수 콤보박스 초기화
            CboPage.Items.Clear();
            for (int i = 1; i <= 121; i++)
            {
                CboPage.Items.Add(i);
            }
            CboPage.SelectedIndex = 0; // 첫 번째 페이지 선택
        }

        private async void BtnSaveData_Click(object sender, RoutedEventArgs e)
        {
            if (GrdResult.Items.Count == 0)
            {
                await this.ShowMessageAsync("저장오류", "실시간 조회후 저장하십시오.");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING))
                {
                    conn.Open();

                    var insRes = 0;
                    foreach (GimhaeFood item in GrdResult.Items)
                    {
                        SqlCommand cmd = new SqlCommand(Models.GimhaeFood.INSERT_QUERY, conn);
                        cmd.Parameters.AddWithValue("@Idx", item.Idx);
                        cmd.Parameters.AddWithValue("@Name", item.Name);
                        cmd.Parameters.AddWithValue("@Category", item.Category);
                        cmd.Parameters.AddWithValue("@Area", item.Area);
                        cmd.Parameters.AddWithValue("@Phone", item.Phone);
                        cmd.Parameters.AddWithValue("@Address", item.Address);
                        cmd.Parameters.AddWithValue("@Businesshour", item.Businesshour);
                        cmd.Parameters.AddWithValue("@Holiday", item.Holiday);
                        cmd.Parameters.AddWithValue("@Menuprice", item.Menuprice);
                        cmd.Parameters.AddWithValue("@Xposition", item.Xposition);
                        cmd.Parameters.AddWithValue("@Yposition", item.Yposition);

                        insRes += cmd.ExecuteNonQuery();
                    }

                    if (insRes > 0)
                    {
                        await this.ShowMessageAsync("저장", "저장성공");
                        StsResult.Content = $"DB저장{insRes}건 성공";
                    }
                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("저장오류", $"저장오류{ex.Message}");
            }
            InitComboDateFormDB();
        }

        private void CboReqDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 콤보박스에서 선택된 카테고리 가져오기
            string selectedCategory = CboReqDate.SelectedItem as string;

            // 선택된 카테고리에 따라 그리드 데이터 필터링
            FilterGridDataByCategory(selectedCategory);
        }

        private void FilterGridDataByCategory(string selectedCategory)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(GrdResult.ItemsSource);
            if (view != null)
            {
                if (string.IsNullOrEmpty(selectedCategory))
                {
                    // 선택된 카테고리가 없을 경우 모든 데이터를 표시
                    view.Filter = null;
                }
                else
                {
                    // 선택된 카테고리에 해당하는 데이터만 표시
                    view.Filter = (item) =>
                    {
                        GimhaeFood foodItem = item as GimhaeFood;
                        return foodItem.Category == selectedCategory;
                    };
                }
            }
        }

        private void GrdResult_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var curItem = GrdResult.SelectedItem as GimhaeFood;

            var mapWin = new MapWin(curItem.Yposition, curItem.Xposition);
            mapWin.Owner = this;
            mapWin.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            mapWin.ShowDialog();
        }

        private async void CboPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedPage = (int)CboPage.SelectedItem;

            string openApiUri = $"http://www.gimhae.go.kr/openapi/tour/restaurant.do?page={selectedPage}";
            string result = string.Empty;

            //WebRequest, WebResponse 객체
            WebRequest req = null;
            WebResponse res = null;
            StreamReader reader = null;

            try
            {
                req = WebRequest.Create(openApiUri);
                res = await req.GetResponseAsync();
                reader = new StreamReader(res.GetResponseStream());
                result = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("오류", $"OpenAPI 조회오류 {ex.Message}");
                return;
            }

            var jsonResult = JObject.Parse(result);
            var record_count = Convert.ToInt32(jsonResult["record_count"]);

            if (record_count == 1206)
            {
                var data = jsonResult["results"];
                var jsonArray = data as JArray; //json자체에서 []안에 들어간 배열데이터만 JArray 변환가능

                var foodGimhae = new List<GimhaeFood>();
                foreach (var item in jsonArray)
                {
                    foodGimhae.Add(new GimhaeFood()
                    {
                        Idx = Convert.ToString(item["idx"]),
                        Name = Convert.ToString(item["name"]),
                        Category = Convert.ToString(item["category"]),
                        Area = Convert.ToString(item["area"]),
                        Phone = Convert.ToString(item["phone"]),
                        Xposition = Convert.ToDouble(item["xposition"]),
                        Yposition = Convert.ToDouble(item["yposition"]),
                        Address = Convert.ToString(item["address"]),
                        Businesshour = Convert.ToString(item["businesshour"]),
                        Holiday = Convert.ToString(item["holiday"]),
                        Menuprice = Convert.ToString(item["menuprice"]),
                    });
                }

                // 가져온 데이터를 그리드에 바인딩
                GrdResult.ItemsSource = foodGimhae;
                StsResult.Content = $"OpenAPI{foodGimhae.Count}건 조회완료";
            }
        }
    }
}