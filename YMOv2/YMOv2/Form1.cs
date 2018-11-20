using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; //random isim oluştururken bazen lazım oluyor :)

namespace YMOv2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            registeredUserBindingSource.AddNew();
            bindingSource1.AddNew();
            realNamesBindingSource.AddNew();
            bindingSource2.AddNew();
        }

        public void noPhoneClink()
        {
            HtmlElementCollection elements = wb1.Document.GetElementsByTagName("span");

            foreach (HtmlElement element in elements)
            {
                try //try ile yapmamın sebebi ise null dönen değerlerde run time hatasını engellemek :D
                {
                    if (element.InnerText.Equals("Telefonum yok"))
                    {
                        element.InvokeMember("click");
                    }
                }catch (Exception){}

            }
        }

        public void KayitOLButon()
        {
            HtmlElementCollection elements = wb1.Document.GetElementsByTagName("span");

            /*          For Each Tara In WebBrowser1.Document.GetElementsByTagName("span")
                        If Tara.GetAttribute("className") = "ui-button-text" And Tara.getAttribute("innerText") = "Kaydol" Then
                        Tara.InvokeMember("click") : Exit For*/

           // < span class="button2__text" data-reactid="132">Kayıt ol</span>

            foreach (HtmlElement element in elements)
            {
                try //try ile yapmamın sebebi ise null dönen değerlerde run time hatasını engellemek :D
                {
                    if (element.InnerText.Equals("Kayıt ol"))
                    {
                        element.InvokeMember("click");
                    }
                }
                catch (Exception) { }

            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            //Program Açıldığında Yandexin Mail Sayfasını Açtırıyoruz
            wb1.Navigate("https://passport.yandex.com.tr/registration/");
            

            YandexMailOpener_dbEntities ymo = new YandexMailOpener_dbEntities();
            bindingSource1.DataSource = ymo.RegisteredUser.ToList(); //kayıtlı userlerin listesini çeker
            bindingSource2.DataSource = ymo.RealNames.ToList(); //kayıtlı Real Fake isimleri çeker
        }

        private void button1_Click(object sender, EventArgs e)
        {
            YandexMailOpener_dbEntities ymo = new YandexMailOpener_dbEntities();
            var userdata = (RegisteredUser)registeredUserBindingSource.Current;
            ymo.RegisteredUser.Add(userdata);
            ymo.SaveChanges();

            bindingSource1.DataSource = ymo.RegisteredUser.ToList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            YandexMailOpener_dbEntities ymo = new YandexMailOpener_dbEntities();
            var namedata = (RealNames)realNamesBindingSource.Current;
            ymo.RealNames.Add(namedata);
            ymo.SaveChanges();

            bindingSource2.DataSource = ymo.RealNames.ToList();
        }

        private void bindingSource2_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void chkRndPass_CheckedChanged(object sender, EventArgs e)
        {
            Random rnd = new Random(); //random kütüphanesi
            int Sayı = rnd.Next(100000, 999999); //6 haneli rastgele bir sayı oluşturduk
            string RandomSonuc = 'A' + Sayı.ToString() + 'z'; //Oluşturduğumuz 6 basamaklı rastgele sayının başına ve sonuna harf ekledik
            txtPassWord.Text = RandomSonuc; //password textine bu yazıyı yazdırdık.
        }

        private void chkRndUserName_CheckedChanged(object sender, EventArgs e)
        {
            Random rnd = new Random(); //random kütüphanesi
            int Sayı = rnd.Next(1, 99); //6 haneli rastgele bir sayı oluşturduk
            string RandomSonuc = Sayı.ToString() +"FDC" ; //Oluşturduğumuz 6 basamaklı rastgele sayının başına ve sonuna harf ekledik
            txtUserName.Text = txtUserName.Text + RandomSonuc; //password textine bu yazıyı yazdırdık.
        }

        //random password olarak kullanılabilir
        //is usefully random password : MessageBox.Show(Path.GetRandomFileName().Replace(".", "").Substring(0, 8));


        private void button3_Click(object sender, EventArgs e)
        {
            
            
            if (txtUserName.Text.Length <= 3)
            {
                MessageBox.Show("Kullanıcı Adı 3 Harften az Olamaz !");
            }
            if (txtPassWord.Text.Length < 8)
            {
                MessageBox.Show("Password 8 Haneden Az Olamaz");
            }

            KayitOLButon();

        }

        private void tmrEnableCheck_Tick(object sender, EventArgs e) //web browserın Yüklenip Yüklenmediğini Görebilmek İçin
            //yüklendikten sonra kendisini devre dışı bırakacaktır.
        {
            if (wb1.ReadyState == WebBrowserReadyState.Complete)
            {
                this.Enabled = true;
                noPhoneClink();
                tmrAdSoyad.Enabled = true;
                tmrEnableCheck.Enabled = false;
            }
            
        }

        public string rName, rSurName;
        public void NameRoll()
        {
            Random rnd = new Random();

            int bilAd = rnd.Next(0, 15);
            int bilSoyad = rnd.Next(0, 15);

            string fakeName = dataGridView2.Rows[bilAd].Cells[1].Value.ToString(); //rastgele isim soy isim oluşturucaz
            string fakeSurname = dataGridView2.Rows[bilSoyad].Cells[2].Value.ToString(); //rastgele isim soy isim oluşturucaz

            rName = fakeName;
            rSurName = fakeSurname;
            if (wb1.Document.GetElementById("firstname").InnerText != "" && wb1.Document.GetElementById("lastname").InnerText != "")
            {
                tmrAdSoyad.Enabled = false;
            }
            
            //MessageBox.Show(fakeName +" "+ fakeSurname);


        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            //kullanıcı adını yazarken isim soy isim yapıştır
            
            /*<input type="text" class="textinput__control" id="firstname" name="firstname" value="" data-reactid="59">*/
            //wb1.Document.GetElementById("firstname").InnerText = txtUserName.Text; //databaseden Çekilecek Ad Kısmına Yazdırma

            /*<input type="text" class="textinput__control" id="lastname" name="lastname" value="" data-reactid="65">*/
            //wb1.Document.GetElementById("lastname").InnerText = txtPassWord.Text; //databaseden çekilecek Soyad Kısmına Yazdırma

            /*< input type = "text" class="textinput__control" id="login" name="login" value="" autocomplete="username" data-reactid="73">*/
            wb1.Document.GetElementById("login").InnerText = txtUserName.Text;


        }

        private void txtPassWord_TextChanged(object sender, EventArgs e)
        {
            /*<input type="password" class="textinput__control" id="password" name="password" value="" autocomplete="new-password" data-reactid="80">*/
            /*<input type="password" class="textinput__control" id="password_confirm" name="password_confirm" value="" autocomplete="new-password" data-reactid="85">*/
            wb1.Document.GetElementById("password").InnerText = txtPassWord.Text;
            // < input type = "text" class="textinput__control" id="hint_answer" name="hint_answer" value="">
            wb1.Document.GetElementById("hint_answer").InnerText = "MercimekDC";

        }

        public void captaCek()
        {

            HtmlElementCollection captaimage = wb1.Document.Images;
            foreach (HtmlElement images in captaimage)
            {
                if (images.GetAttribute("src").Contains("https://ext.captcha.yandex.net/image?"))
                {
                    pictureBox1.Load(images.GetAttribute("src"));
                }
                    

            }
        }
               
        private void tmrAdSoyad_Tick(object sender, EventArgs e)
        {
            NameRoll();
            captaCek();
        }

        private void button4_Click(object sender, EventArgs e)
        {
        }

        private void txtCapta_TextChanged(object sender, EventArgs e)
        {
            //< input type = "text" class="textinput__control" id="captcha" name="captcha" value="">
            wb1.Document.GetElementById("password_confirm").InnerText = txtPassWord.Text; //2. password yerine kayıt etmeden önce doldurmak gerek
            wb1.Document.GetElementById("captcha").InnerText = txtCapta.Text;
        }

        private void chk1_CheckedChanged(object sender, EventArgs e)
        {
            wb1.Document.GetElementById("firstname").InnerText = rName;
            
        }

        private void chk2_CheckedChanged(object sender, EventArgs e)
        {
            wb1.Document.GetElementById("lastname").InnerText = rSurName;
        }

        private void chk3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chk4_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
