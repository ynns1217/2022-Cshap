using System;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace mook_CarInfo
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Form1 클래스의 생성자
        /// </summary>
        public Form1()
        {
            // Form 위에 있는 각종 컨트롤 및 컴포넌트들의 객체 생성, 초기화 작업등을 수행.
            InitializeComponent();
        }

        // MS SQL 서버 접속 정보 문자열 (윈도우 인증 => Integrated Security=true)
        // Integrated Security=true => 윈도우 인증
        // Integrated Security=false => SQL 계정
        // Server=(local) or Server=127.0.0.1 => 접속 서버 정보
        // database=ADOTest => 데이터베이스 명
        private string connectionStr = "Server=(local);database=ADOTest;" +
                "Integrated Security=true";

        /// <summary>
        /// 폼이 로드되는 이벤트에서 listView_initialize() 메소드를 호출하여 
        /// 등록된 차량 정보를 먼저 조회하여, 화면의 조회 결과 리스트에 표시.
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            // 리스트 뷰를 갱신함.
            listView_initialize();
        }

        /// <summary>
        /// 등록된 차량 정보를 모두 조회하고, 조회된 전체 데이터를 리스트 뷰에 출력함.
        /// </summary>
        private void listView_initialize()
        {
            // 리스트 뷰 초기화. ( 리스트 뷰에 표시된 데이터(items 컬렉션)를 모두 삭제함. )
            this.lvList.Items.Clear();

            // 데이터 베이스 접속 객체 생성, 아직 데이터 베이스 연결전임.
            //접속을 위한 정보를 가지고 있는 객체라고생각을 하면된다. commectionStr
            // 접속 객체를 만드는 단계
            var Conn = new SqlConnection(connectionStr);

            // 데이터베이스 접속 및 사용시 발생하는 Exception 처리.
            try
            {
                // 데이터베이스 연결
                // sqlconnection의 접속 매소드인 open사용
                Conn.Open();

                // SqlCommand(CURD공통)  ,  클래스를 사용하여 실행할 SQL 설정 
                // R을 실행하는 객체 매소드가 따로있고, CUD를 하는 전용매소드가 따로있음
                // 매개변수1 : 실행할 SQL 문자열
                // 매개변수2 : 연결된 데이터베이스의 SqlConnection 객체
                var Comm = new SqlCommand("Select * From TB_CAR_INFO", Conn);

                // SqlCommand 클래스에 초기화한 SQL 문장을 실행하고
                // 실행 결과를 SqlDataReader 로 반환.
                // 매개변수1 : CommandBehavior.CloseConnection 로 설정하면,
                //           SqlDataReader 를 close 할 때,
                //           데이터베이스의 접속 객체인 SqlConnection 도 닫음.
                //           따라서, Conn.Close(); 를 하지 않아도 됨
                //R 이기 때문에 반환을 한다. SQL데이터리더형으로 반환
                var myRead = Comm.ExecuteReader(CommandBehavior.CloseConnection);

                // 반환된 조회 결과의 첫 번째 데이터 전에 위치하고 있어,
                // SqlDataReader의 Read() 함수를 사용하여 첫 번째 데이터를 읽어옴.
                while (myRead.Read()) //SQL리드 = MYREAD ,시작위치는 첫번째데이터 전에 있음,
                    //한행 한행 읽는다고 생각을 하면된다.(myRead가 호출이 되면) 
                {
                    // 읽어온 한 행의 데이터에서 테이블의 각 칼럼의 이름으로 각각의 데이터를 읽어옴.

                    // 읽어온 각각의 행 데이터를 화면의 리스트뷰에 출력하기 위해, 리스트뷰의 데이터인 ListViewItem 로 
                    // 만들기 위해서 각 칼럼의 데이터를 문자열로 만듬.
                    // => ListViewItem 생성자의 매개변수는 string[] 형태임., string 배열을 만듬
                    var strArray = new String[] { myRead["id"].ToString(),
                        myRead["carName"].ToString(), 
                        myRead["carYear"].ToString(),
                        myRead["carPrice"].ToString(), 
                        myRead["carDoor"].ToString() };

                    // 읽어온 한 행의 데이터를 ListViewItem 형태로 초기화.
                    var lvt = new ListViewItem(strArray);

                    // 리스트 뷰에 표시할 데이터로 초기화된 ListViewItem 을 리스트 뷰 데이터로 추가함.
                    this.lvList.Items.Add(lvt); //리스트뷰에 한 행을 추가한다는 의미의 문장
                }

                // 조회된 모든 데이터를 읽은 후에, 
                // SqlDataReader 및 SqlConnection 자원을 모두 Close 함.
                myRead.Close();
                //Conn.Close(); 70번째줄에 보면 자동으로 닫게 코딩이 되어있음
            }
            catch (Exception ex)
            {
                // 발생한 Exception(데이터 베이스 사용 중 에러, SQL 문법 에러 등등) 처리하여 정상 종료가 되도록 함.

                // 매개변수 1 : MessageBox에 표시할 메세지 본문
                // 매개변수 2 : MessageBox에 표시할 타이틀
                // 매개변수 3 : MessageBox에 표시할 버튼
                // 매개변수 4 : MessageBox에 표시할 아이콘

                MessageBox.Show("프로그램 실행중에 문제가 발생해서, 프로그램을 종료합니다. \r\n\r\n" + ex.Message, "에러",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Conn.Close();
                Application.Exit();
            }

        }

        /// <summary>
        /// 차량 신규 데이터를 입력 후, 데이터베이스에 저장함.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // 모든 입력 항목에 데이터가 입력이 되었는지 체크
            // 입력하지 않은 항목이 입으면, 메세지 박스로 알림.
            if (!textBox_DataChk()) //하나의 항목에 데이터 입력이 누락된경우 -> !false가 리턴 =>true가 리턴
                //모든항목이 정상입력 => true가 리턴 => !true => false가 된다.
            {
                return;
            }

            var Conn = new SqlConnection(connectionStr); //false가 된경우

            try
            {
                // 데이터베이스 연결
                Conn.Open();

                // 데이터 베이스에서 SQL 문장 컴파일을 최초 1회만 수행하고, 문제가 없다면
                // 앞으로는 컴파일된 SQL 문장을 재사용하여, @ 로 시작하는 매개변수의 실제값만
                // 변경 적용되도록 SQL 문장을 작성.
                string Sql = "insert into TB_CAR_INFO(carName, carYear, carPrice, carDoor) "
                                + "values( @carName, @carYear, @carPrice, @carDoor )";

                // 실행할 insert SQL 문장으로 SqlCommand 객체 생성.
                var Comm = new SqlCommand(Sql, Conn);

                // 위의 insert 문장으로 초기화된 SqlCommand 객체에서 사용할 매개변수를 각각 설정함.
                Comm.Parameters.Add("@carName", SqlDbType.NVarChar, 30);
                Comm.Parameters.Add("@carYear", SqlDbType.VarChar, 4);
                Comm.Parameters.Add("@carPrice", SqlDbType.Int);
                Comm.Parameters.Add("@carDoor", SqlDbType.Int);

                // 위의 @ 로 시작하는 매개변수를 실제 값으로 적용.
                Comm.Parameters["@carName"].Value = this.txtName.Text;
                Comm.Parameters["@carYear"].Value = this.txtYear.Text;

                // 차량 가격은 데이터베이스에 정수 타입임으로 문자열에서 정수 타입으로 변경.
                // Convert 클래스의 ToInt32() 메소드를 사용하면 됨. 
                // ToInt32 는 32bit 정수로 변경함. 즉, 4바이트 형태의 정수임으로 일반적인 int 형임.
                Comm.Parameters["@carPrice"].Value = Convert.ToInt32(this.txtPrice.Text);
                Comm.Parameters["@carDoor"].Value = Convert.ToInt32(this.txtDoor.Text);

                /*string Sql = "insert into TB_CAR_INFO(carName, carYear, carPrice, carDoor) values('";
                Sql += this.txtName.Text + "'," + this.txtYear.Text + "," +
                    Convert.ToInt32(this.txtPrice.Text) + "," + Convert.ToInt32(this.txtDoor.Text) + ")";
                var Comm = new SqlCommand(Sql, Conn);*/

                // Insert, Update, Delete SQL 문장은 SqlCommand 클래스의 ExecuteNonQuery() 메소드를 사용하여 
                // SQL 문장을 실행함.
                // 반환되는 값은 영향을 받은 행수임.
                // 반환되는 것은 행수뿐! 그 외는  반환할것이 없음!
                int insertRowCnt = Comm.ExecuteNonQuery();

                //데이터 베이스 접속을 종료함.
                Conn.Close();

                // 데이터베이스에 영향을 받은 행이 한 건이면 정상적으로 insert가 된 것임.
                if (insertRowCnt == 1)
                {
                    MessageBox.Show("정상적으로 데이터가 저장되었습니다.", "저장",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 신규 데이터가 저장이 되었으므로, 리스트 뷰를 갱신함.
                    listView_initialize();

                    // 입력 항목의 기 입력된 데이터는 모두 삭제함.
                    textBox_Data_Initialize();
                }
                else
                {
                    MessageBox.Show("정상적으로 데이터가 저장되지 않았습니다.", "에러",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("프로그램 실행중에 문제가 발생했습니다.  \r\n\r\n" + ex.Message, "에러",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                //데이터 베이스 접속을 종료함.
                Conn.Close();
            }
        }

        /// <summary>
        /// 입력 항목에 입력된 데이터를 모두 삭제함.
        /// </summary>
        private void textBox_Data_Initialize() // 186항과 연결이되어서 필요없어진 데이터를 삭제하는 이니셜라이징
        {
            this.txtName.Clear();
            this.txtYear.Clear();
            this.txtPrice.Clear();
            this.txtDoor.Clear();
        }

        /// <summary>
        /// 리스트 뷰의 조회된 검색 결과에서 한 행을 선택하면, 
        /// 선택된 행의 각 항목별 데이터를 수정할 수 있도록
        /// 리스트 뷰의 각 항목별 데이터를 각 입력 항목(TextBox)의 값으로 설정함.
        /// </summary>
        private void lvList_Click(object sender, EventArgs e)
        {
            // 리스트 뷰의 조회된 결과에서 선택된 데이터가 있음.
            if (this.lvList.SelectedItems.Count > 0)
            {
                // SelectedItems[0] 의미. => 리스트 뷰에서 여러 행을 선택했다면, 선택된 행 중에서 첫 번째 행.
                // SubItems[1] => 선택된 행에서 두 번째 항목. => "이름" 항목
                // SubItems[2] => 선택된 행에서 세 번째 항목. => "년도" 항목
                this.txtName.Text = this.lvList.SelectedItems[0].SubItems[1].Text;
                this.txtYear.Text = this.lvList.SelectedItems[0].SubItems[2].Text;
                this.txtPrice.Text = this.lvList.SelectedItems[0].SubItems[3].Text;
                this.txtDoor.Text = this.lvList.SelectedItems[0].SubItems[4].Text;

                /*MessageBox.Show(this.lvList.SelectedItems[0].SubItems[0].Text + "\r\n"
                    + lvList.SelectedItems[0].SubItems[1].Text + "\r\n"
                    + lvList.SelectedItems[0].SubItems[2].Text + "\r\n" 
                    + lvList.SelectedItems[0].SubItems[3].Text + "\r\n"
                    + lvList.SelectedItems[0].SubItems[4].Text);*/
            }
        }

        /// <summary>
        /// 입력항목의 수정된 차량 데이터를 데이터베이스에 저장함.
        /// </summary>
        private void btnModify_Click(object sender, EventArgs e)
        {
            // 데이터 수정을 위해서 리스트 뷰에서 데이터를 선택했는지 확인
            //248~257까지가 수정을 할지말지를 결정하는 부분
            if (!listView_Secected_Row_Check())
            {
                return;
            }

            // 입력 항목에 데이터가 입력되지 않은 항목이 있는지 확인
            if (!textBox_DataChk())
            {
                return;
            }

            // 접속할 데이터베이스의 SqlConnection 객체 생성.
            var Conn = new SqlConnection(connectionStr);

            try
            {
                // 데이터베이스 연결
                Conn.Open();

                // 데이터 베이스에서 SQL 문장 컴파일을 최초 1회만 수행하고, 문제가 없다면
                // 앞으로는 컴파일된 SQL 문장을 재사용하여, @ 로 시작하는 매개변수의 실제값만
                // 변경 적용되도록 SQL 문장을 작성.
                string Sql = "update TB_CAR_INFO "
                        + "set carName = @carName, carYear = @carYear, "
                        + "carPrice = @carPrice, carDoor = @carDoor "
                        + "where id = @id ";

                // 실행할 update SQL 문장으로 SqlCommand 객체 생성.
                var Comm = new SqlCommand(Sql, Conn);

                // 위의 update 문장으로 초기화된 SqlCommand 객체에서 사용할 매개변수를 각각 설정함.
                Comm.Parameters.Add("@id", SqlDbType.Int);
                Comm.Parameters.Add("@carName", SqlDbType.NVarChar, 30);
                Comm.Parameters.Add("@carYear", SqlDbType.VarChar, 4);
                Comm.Parameters.Add("@carPrice", SqlDbType.Int);
                Comm.Parameters.Add("@carDoor", SqlDbType.Int);

                // 위의 @ 로 시작하는 매개변수를 실제 값으로 적용.
                Comm.Parameters["@id"].Value =
                    //식별이 될수있는 값인 id를 가지고 수정을 해야한다. 따라서 0인덱스를 가지고온다
                    Convert.ToInt32(this.lvList.SelectedItems[0].SubItems[0].Text); 
                Comm.Parameters["@carName"].Value = this.txtName.Text;
                Comm.Parameters["@carYear"].Value = this.txtYear.Text;
                Comm.Parameters["@carPrice"].Value = Convert.ToInt32(this.txtPrice.Text);
                Comm.Parameters["@carDoor"].Value = Convert.ToInt32(this.txtDoor.Text);

                // Insert, Update, Delete SQL 문장은 SqlCommand 클래스의 ExecuteNonQuery() 메소드를 사용하여 
                // SQL 문장을 실행함.
                // 반환되는 값은 영향을 받은 행수임.
                int updateRowCnt = Comm.ExecuteNonQuery();

                //데이터 베이스 접속을 종료함.
                Conn.Close();

                // 데이터베이스에 영향을 받은 행이 한 건이면 정상적으로 update가 된 것임.
                if (updateRowCnt == 1)
                {
                    MessageBox.Show("정상적으로 데이터가 수정되었습니다.", "수정",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 데이터가 수정이 되었으므로, 리스트 뷰를 갱신함.
                    listView_initialize();

                    // 입력 항목의 기 입력된 데이터는 모두 삭제함.
                    textBox_Data_Initialize();
                }
                else
                {
                    MessageBox.Show("정상적으로 데이터가 수정되지 않았습니다.", "에러",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("프로그램 실행중에 문제가 발생했습니다.  \r\n\r\n" + ex.Message, "에러",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                //데이터 베이스 접속을 종료함.
                Conn.Close();
            }
        }

        /// <summary>
        /// 입력항목에 입력된 값으로 조건 검색을 함.
        /// </summary>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.lvList.Items.Clear();
            var Conn = new SqlConnection(connectionStr);

            try
            {
                // 데이터베이스 연결
                Conn.Open();

                // 데이터 베이스에서 SQL 문장 컴파일을 최초 1회만 수행하고, 문제가 없다면
                // 앞으로는 컴파일된 SQL 문장을 재사용하여, @ 로 시작하는 매개변수의 실제값만
                // 변경 적용되도록 SQL 문장을 작성.
                string Sql = "Select * From TB_CAR_INFO "
                            + "where carName = @carName or carYear = @carYear "
                            + "or carPrice = @carPrice or carDoor = @carDoor ";

                // 실행할 select SQL 문장으로 SqlCommand 객체 생성.
                var Comm = new SqlCommand(Sql, Conn);

                // 위의 select 문장으로 초기화된 SqlCommand 객체에서 사용할 매개변수를 각각 설정함.
                Comm.Parameters.Add("@carName", SqlDbType.NVarChar, 30);
                Comm.Parameters.Add("@carYear", SqlDbType.VarChar, 4);
                Comm.Parameters.Add("@carPrice", SqlDbType.Int);
                Comm.Parameters.Add("@carDoor", SqlDbType.Int);

                // 위의 @ 로 시작하는 매개변수를 실제 값으로 적용.
                Comm.Parameters["@carName"].Value = this.txtName.Text;
                Comm.Parameters["@carYear"].Value = this.txtYear.Text;

                // 검색 조건으로 사용되는 차량 가격과 차량 문의 수는 
                // 입력된 데이터가 정수 이외이면, Exception 처리하고 메세지로 알려줌.
                try
                {
                    // 차량 가격과 차량 문의 수에 입력된 데이터가 없으며, 각각 0 으로 초기화함.
                    // 무조건 try catch를 해야한다!!! 
                    Comm.Parameters["@carPrice"].Value =
                    Convert.ToInt32((this.txtPrice.Text == "") ? 0 : Convert.ToInt32(this.txtPrice.Text));
                    Comm.Parameters["@carDoor"].Value =
                        Convert.ToInt32((this.txtDoor.Text == "") ? 0 : Convert.ToInt32(this.txtDoor.Text));
                }
                catch (Exception)
                {
                    //가격, 차문 수 입력 항목에서 숫자 이외의 데이터가 입력이 되면, 예외로 처리함.
                    MessageBox.Show("가격, 차문 수의 입력 항목에 숫자 이외의 데이터가 있습니다. \r\n다시 확인해주세요.", "에러",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //데이터 베이스 접속을 종료함.
                    Conn.Close();

                    return;
                }



                /*var Comm = new SqlCommand("Select * From TB_CAR_INFO where carName = '" + this.txtName.Text + 
                    "' or carYear = '" + this.txtYear.Text + 
                    "' or carPrice = "
                    + Convert.ToInt32((this.txtPrice.Text == "") ? 0 : Convert.ToInt32(this.txtPrice.Text)) + 
                    " or carDoor = "
                    + Convert.ToInt32((this.txtDoor.Text == "") ? 0 : Convert.ToInt32(this.txtDoor.Text)), Conn);*/

                // SqlCommand 클래스에 초기화한 SQL 문장을 실행하고
                // 실행 결과를 SqlDataReader 로 반환.
                // 매개변수1 : CommandBehavior.CloseConnection 로 설정하면,
                //           SqlDataReader 를 close 할 때,
                //           데이터베이스의 접속 객체인 SqlConnection 도 닫음.
                //           따라서, Conn.Close(); 를 하지 않아도 됨.
                var myRead = Comm.ExecuteReader(CommandBehavior.CloseConnection);
                //var myRead = Comm.ExecuteReader();

                // 반환된 조회 결과의 첫 번째 데이터 전에 위치하고 있어,
                // SqlDataReader의 Read() 함수를 사용하여 첫 번째 데이터를 읽어옴.
                while (myRead.Read())
                {
                    // 읽어온 한 행의 데이터에서 테이블의 각 칼럼의 이름으로 각각의 데이터를 읽어옴.

                    // 읽어온 각각의 행 데이터를 화면의 리스트뷰에 출력하기 위해, 리스트뷰의 데이터인 ListViewItem 로 
                    // 만들기 위해서 각 칼럼의 데이터를 문자열로 만듬.
                    // => ListViewItem 생성자의 매개변수는 string[] 형태임.
                    var strArray = new String[] { myRead["id"].ToString(),
                        myRead["carName"].ToString(), 
                        myRead["carYear"].ToString(),
                        myRead["carPrice"].ToString(), 
                        myRead["carDoor"].ToString() };

                    // 읽어온 한 행의 데이터를 ListViewItem 형태로 초기화.
                    var lvt = new ListViewItem(strArray);

                    // 리스트 뷰에 표시할 데이터로 초기화된 ListViewItem 을 리스트 뷰 데이터로 추가함.
                    this.lvList.Items.Add(lvt);
                }

                // 조회된 모든 데이터를 읽은 후에, 
                // SqlDataReader 및 SqlConnection 자원을 모두 Close 함.
                myRead.Close();
                //Conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("프로그램 실행중에 문제가 발생했습니다.  \r\n\r\n" + ex.Message, "에러",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                //데이터 베이스 접속을 종료함.
                Conn.Close();
            }
        }

        /// <summary>
        /// 리스트 뷰에서 선택된 데이터를 삭제함.
        /// </summary>
        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 리스트 뷰의 조회된 결과에서 선택된 데이터가 있음.
            if (this.lvList.SelectedItems.Count > 0)
            {
                // 메세지 박스에서 표시되는 두 개의 버튼중에서 선택된 버튼에 따라 구분해서 처리함.
                // => 선택된 버튼에 따라 두 가지의 값이 return 이 됨.
                // DialogResult.Yes( Yes 버튼 ), DialogResult.No( No 버튼 )

                // 매개변수 1 : MessageBox에 표시할 메세지 본문
                // 매개변수 2 : MessageBox에 표시할 타이틀
                // 매개변수 3 : MessageBox에 표시할 Yes 버튼, No 버튼
                // 매개변수 4 : MessageBox에 표시할 아이콘
                DialogResult dlr = MessageBox.Show("데이터를 삭제할까요?", "삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                // Yes 버튼을 선택한 경우. => 리스트 뷰에서 선택한 데이터의 삭제 처리를 진행함.
                if (dlr == DialogResult.Yes)
                {
                    // 데이터 베이스 접속 객체 생성, 아직 데이터 베이스 연결전임.
                    var Conn = new SqlConnection(connectionStr);

                    try
                    {
                        // 데이터베이스 연결
                        Conn.Open();

                        // 데이터 베이스에서 SQL 문장 컴파일을 최초 1회만 수행하고, 문제가 없다면
                        // 앞으로는 컴파일된 SQL 문장을 재사용하여, @ 로 시작하는 매개변수의 실제값만
                        // 변경 적용되도록 SQL 문장을 작성.
                        string Sql = "delete from TB_CAR_INFO "
                            + "where id = @id ";

                        // 실행할 select SQL 문장으로 SqlCommand 객체 생성.
                        var Comm = new SqlCommand(Sql, Conn);

                        // 위의 delete 문장으로 초기화된 SqlCommand 객체에서 사용할 매개변수를 각각 설정함.
                        Comm.Parameters.Add("@id", SqlDbType.Int);

                        // 위의 @ 로 시작하는 매개변수를 실제 값으로 적용.
                        // 리스트 뷰에서 선택한 행의 첫 번째 항목인 차량 번호 항목의 데이터를 삭제 조건으로 사용.
                        Comm.Parameters["@id"].Value =
                            Convert.ToInt32(this.lvList.SelectedItems[0].SubItems[0].Text);

                        /*string Sql = "delete from TB_CAR_INFO where id = " + Convert.ToInt32(this.lvList.SelectedItems[0].SubItems[0].Text) + "";
                        var Comm = new SqlCommand(Sql, Conn);*/

                        // Insert, Update, Delete SQL 문장은 SqlCommand 클래스의 ExecuteNonQuery() 메소드를 사용하여 
                        // SQL 문장을 실행함.
                        // 반환되는 값은 영향을 받은 행수임.
                        int deleteRowCnt = Comm.ExecuteNonQuery();

                        //데이터 베이스 접속을 종료함.
                        Conn.Close();

                        // 데이터베이스에 영향을 받은 행이 한 건이면 정상적으로 delete가 된 것임.
                        if (deleteRowCnt == 1)
                            MessageBox.Show("데이터가 정상적으로 삭제되었습니다.", "삭제", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("데이터를 삭제하지 못하였습니다.", "삭제", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        // 입력 항목의 기 입력된 데이터는 모두 삭제함.
                        textBox_Data_Initialize();

                        // 데이터가 삭제 되었으므로, 리스트 뷰를 갱신함.
                        listView_initialize();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("프로그램 실행중에 문제가 발생했습니다.  \r\n\r\n" + ex.Message, "에러",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //데이터 베이스 접속을 종료함.
                        Conn.Close();
                    }
                }
            }
            else
                MessageBox.Show("삭제할 행을 선택하세요.", "삭제",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnAllSearch_Click(object sender, EventArgs e)
        {
            // 입력 항목의 기 입력된 데이터는 모두 삭제함.
            textBox_Data_Initialize();

            // 리스트 뷰를 갱신함.
            listView_initialize();
        }

        /// <summary>
        /// 입력 항목에 모든 데이터가 입력이 되었는지 확인함.
        /// 모두 입력이 된 상태이면, true를 반환함. 그렇지 않으면, false 를 반환함.
        /// </summary>
        private bool textBox_DataChk()  //바디 부분(120번줄 254번줄과 연관)
        {
            if (this.txtName.Text != "" && this.txtYear.Text != "" &&
                this.txtPrice.Text != "" && this.txtDoor.Text != "")
                return true;
            else
            {
                MessageBox.Show("입력 항목의 데이터를 확인해주세요.", "입력 항목 체크",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return false;
            }
        }

        /// <summary>
        /// 등록된 데이터 수정, 삭제시 리스트 뷰에서 데이터가 선택이 되었는지를 확인함.
        /// 선택이 된 상태이면, true 를 반환하고, 
        /// 선택이 되지 않은 상태이면, false 를 반환함.
        /// </summary>
        private bool listView_Secected_Row_Check() //247번째 줄과 연관
        {
            if (this.lvList.SelectedItems.Count > 0)
                return true;
            else
                MessageBox.Show("선택된 데이터가 없습니다. 데이터를 선택해주세요.", "데이터 선택",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

            return false;
        }
    }
}