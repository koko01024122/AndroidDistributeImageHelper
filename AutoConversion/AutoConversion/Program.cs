using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoConversion
{
    class Program
    {
        
        static IList<logoimage> logoList = new List<logoimage>();
        static IList<advertising> advList = new List<advertising>();
        private static String openUrl;
        [STAThread]
        static void Main(string[] args)
        {
            Console.Title = "Android应用分发图片自动转化程序";
            
            Console.Write("****************************欢迎使用Android应用分发图片自动转化程序**************************\n");
            Console.Write("*     请输入您要处理的类型                                                                  *\n");
            Console.Write("*     1.应用商城logo                                                                        *\n");
            Console.Write("*     2.展示图片                                                                            *\n");
            Console.Write("*     3.我都要                                                                              *\n");
            Console.Write("*     输入后请根据相应的提示选择原图片                                                      *\n");
            Console.Write("*                                                                                           *\n");
            Console.Write("*                  项目地址:https://github.com/koko01024122/AndroidDistributeImageHelper   *\n");
            Console.Write("*                  作者：NOspy                                                              *\n");
            Console.Write("*                  版本：V1.0                                                               *\n");
            Console.Write("*********************************************************************************************\n");

            while (true)
            {
                String userInput = Console.ReadLine();
                if (userInput.Equals("1"))
                {
                    handleLogos();
                    break;
                }
                else if (userInput.Equals("2"))
                {
                    handleAds();
                    break;
                }
                else if (userInput.Equals("3"))
                {
                    handleLogos();
                    handleAds();

                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("您的输入有误，请重新输入");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }

                break;
            }
            if (openUrl!=null)
            {
                System.Diagnostics.Process.Start(@openUrl);
                Console.Write("全部文件处理完毕，按任意键退出");
            }

            Console.Write("按任意键退出");
            Console.ReadKey();
        }

        static void outPutAdvs(String file, String path, String filename)
        {
            initAds();
            for (int i = 0; i < advList.Count; i++)
            {
                Console.WriteLine("---------------------正在处理" + advList[i].name.Replace(".png", "").Replace(".jpg", "") + "第" + i + "张"  + "\n");
                save(ConvertBitmapToScreen(file, advList[i].width, advList[i].height), advList[i].name + filename,
                    path);
            }
        }


        static void outPutLogos(String file, String path)
        {
            initLogos();
            for (int i = 0; i < logoList.Count; i++)
            {
                Console.Write("---------------------正在处理" + logoList[i].name.Replace(".png", "").Replace(".jpg","") +"第"+i+"张"+ "\n");
                save(ConvertBitmapToScreen(file, logoList[i].size, logoList[i].size), logoList[i].name, path);
            }
        }



        static OpenFileDialog selectFile(int number)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (number != 1)
            {
                ofd.Multiselect = true;
                ofd.Title = "请选择您要转换的"+number+"张图片";
            }
            else
            {
                ofd.Title = "请选择您要转换的图片";
               
            }
            ofd.InitialDirectory = "c:\\";
            ofd.Filter = "图片文件(*.png;*.jpg)|*.png;*.jpg|所有文件|*.*";
            ofd.ValidateNames = true;
      
            ofd.CheckPathExists = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string[] strFileName = ofd.FileNames;
                for (int i = 0; i < strFileName.Length; i++)
                {
                    Console.WriteLine("文件路径：" + strFileName[i]);
                }

                //其他代码
                return ofd;
            }
            else
            {
                return null;
            }
        }


        static void save(Bitmap bmp, String name, String path)
        {
            if (name.Contains("360") && name.Contains("图标"))
            {
                Console.WriteLine("---------------------正在给360切圆脚（误）圆角");
                Color color = Color.FromArgb(0, 0, 0, 0);
             //   Console.WriteLine("本图片尺寸:"+bmp.);
                bmp = RoundCorners(bmp, 70, color);
                
            }
            // pictureBox1.DrawToBitmap(bmp, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));

            bmp.Save(path + name, System.Drawing.Imaging.ImageFormat.Png);
        }

        public static void CreateDir(string subdir, String paths)
        {
            string path = paths + "/" + subdir;
            if (Directory.Exists(path))
            {
                //  Console.WriteLine("此文件夹已经存在，无需创建！");
            }
            else
            {
                Directory.CreateDirectory(path);
                Console.WriteLine(path + " ------------------文件目录创建成功!->");
            }
        }


        public static Bitmap ReadImageFile(string path)
        {
            FileStream fs = File.OpenRead(path); //OpenRead
            int filelength = 0;
            filelength = (int) fs.Length; //获得文件长度 
            Byte[] image = new Byte[filelength]; //建立一个字节数组 
            fs.Read(image, 0, filelength); //按字节流读取 
            System.Drawing.Image result = System.Drawing.Image.FromStream(fs);
            fs.Close();
            Bitmap bit = new Bitmap(result);
            return bit;
        }


        private static Bitmap ConvertBitmapToScreen(string strBitmapPath, int iBitmapWidth, int iBitmapHeight)
        {
            //装载图片  
            System.Drawing.Image image = System.Drawing.Image.FromFile(strBitmapPath);

            //获取图片的实际宽度与高度  
            int srcWidth = image.Width;
            int srcHeight = image.Height;

            if (iBitmapHeight == 0 && iBitmapWidth == 0)
            {
                return null;
            }

            //创建Bitmap对象，并设置Bitmap的宽度和高度。  
            Bitmap bmp = new Bitmap(iBitmapWidth, iBitmapHeight);

            //从Bitmap创建一个System.Drawing.Graphics对象，用来绘制高质量的缩小图。  
            System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(bmp);
            //设置 System.Drawing.Graphics对象的SmoothingMode属性为HighQuality  
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //下面这个也设成高质量  
            gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            //下面这个设成High  
            gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //把原始图像绘制成上面所设置宽高的缩小图  
            System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, iBitmapWidth, iBitmapHeight);
            gr.DrawImage(image, rectDestination, 0, 0, srcWidth, srcHeight, GraphicsUnit.Pixel);

            image.Dispose();

            return bmp;
        }

        static void handleLogos()
        {
        
            OpenFileDialog daDialog = selectFile(1);
            if (daDialog == null)
            {
                while (true)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("您未选取任何图片，是否重新选择");
                    Console.WriteLine("(y/n)");
                    Console.ForegroundColor = ConsoleColor.White;
                    String yesorno = Console.ReadLine();
                    if (yesorno.Equals("y") || yesorno.Equals("Y"))
                    {
                        daDialog = selectFile(1);
                        if (daDialog == null)
                        {
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("-----------------------Logo部分开始处理-------------------------------");
                            String url = System.IO.Path.GetDirectoryName(daDialog.FileName) + "\\";
                            //    Console.WriteLine("文件路径"+url);
                            
                            Console.WriteLine("≧∇≦ 抓住文件：" + daDialog.FileName);
                            openUrl = url;
                            CreateDir("自动生成logo", url);
                            outPutLogos(daDialog.FileName, url + "自动生成Logo\\");
                            Console.WriteLine("-----------------------Logo部分处理完成-------------------------------");
                            break;
                        }
                    }
                    else if (yesorno.Equals("n")|| yesorno.Equals("N"))
                    {
                        Console.WriteLine("程序即将退出");
                        Thread.Sleep(200);
                        Application.Exit();
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            else
            {
                Console.WriteLine("-----------------------Logo部分开始处理-------------------------------");
                String url = System.IO.Path.GetDirectoryName(daDialog.FileName) + "\\";
                //    Console.WriteLine("文件路径"+url);
                openUrl = url;
                Console.WriteLine("≧∇≦ 抓住文件：" + daDialog.FileName);
                CreateDir("自动生成logo", url);
                outPutLogos(daDialog.FileName, url + "自动生成Logo\\");
                Console.WriteLine("-----------------------Logo部分处理完成-------------------------------");
            }
        }

        static void handleAds()
            //处理宣传图部分
        {
            String numString;
            Boolean once = true;
            int number;

            while (true)
            {
                if (once)
                {
                    Console.WriteLine("请输入您要处理宣传图的数量");
                }
                numString = Console.ReadLine();

                if (numString.Equals("0"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("您的输入有误，请重新输入");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    try
                    {
                        number = int.Parse(numString);
                    }
                    catch (Exception e)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("您的输入有误，请重新输入");
                        Console.ForegroundColor = ConsoleColor.White;
                        continue;
                        throw;
                    }
                    break;
                }
            }
            OpenFileDialog daDialog = selectFile(number);
            String[] names;

            if (daDialog == null)
            {
                while (true)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("您未选取任何图片，是否重新选择");
                    Console.WriteLine("(y/n)");

                    Console.ForegroundColor = ConsoleColor.White;
                    String yesorno = Console.ReadLine();
                    if (yesorno.Equals("y") || yesorno.Equals("Y"))
                    {
                        daDialog = selectFile(number);
                        if (daDialog == null)
                        {
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else if (yesorno.Equals("n"))
                    {
                        Console.WriteLine("程序即将退出");
                        Thread.Sleep(200);
                        Application.Exit();
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                if (daDialog!=null)
                {
                    names = daDialog.FileNames;
                    imageNumberCheck(names, daDialog, number);

                }

            }
            else
            {
                names = daDialog.FileNames;
                imageNumberCheck(names, daDialog, number);
                Console.WriteLine("-----------------------宣传图部分开始处理------------------------------");
                //TODO://重构此项目


                String url = System.IO.Path.GetDirectoryName(daDialog.FileName) + "\\";
                openUrl = url;
                if (names.Length != 0)
                {
                    for (int i = 0; i < names.Length; i++)
                    {
                        CreateDir("自动生成宣传图", url);
                        outPutAdvs(names[i], url + "自动生成宣传图\\", names[i].Replace(url, ""));

                    }

                    Console.WriteLine("-----------------------宣传图部分处理完成------------------------------");

                }

                //    Console.WriteLine("文件路径"+url);
                //Console.WriteLine("≧∇≦ 抓住文件：" + daDialog.FileName);
            }
        
        }


        static void initAds()
        {
            advertising yybAds1 = new advertising("应用宝", 800, 480, 1024);
            advList.Add(yybAds1);

            advertising Ads1360 = new advertising("360", 800, 480, 3072);
            advList.Add(Ads1360);
            advertising azAds1 = new advertising("安智市场", 800, 480, 2048);
            advList.Add(azAds1);

            advertising lxAds1 = new advertising("华为", 800, 480, 2048);
            advList.Add(lxAds1);
            advertising jlAds1 = new advertising("金立", 800, 480, 1024);
            advList.Add(jlAds1);
        }


        static void imageNumberCheck(String[] names, OpenFileDialog daDialog, int number)
        {
            while (true)
            {
                names = daDialog.FileNames;
                if (number < names.Length)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("------------------您选择的图片数量大于您需求的图片数量，是否重新选择");
                    Console.WriteLine("------------------重新选择会覆盖之前选择的内容");
                    Console.WriteLine("(y/n)");
                    Console.ForegroundColor = ConsoleColor.White;
                    String yesorno = Console.ReadLine();
                    if (yesorno.Equals("y") || yesorno.Equals("Y"))
                    {
                        daDialog = selectFile(number);
                    }
                    else if (yesorno.Equals("n") || yesorno.Equals("N"))
                    {
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (number > names.Length)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("您选择的图片数量小于您需求的图片数量，是否重新选择");
                    Console.WriteLine("(y/n)");
                    Console.ForegroundColor = ConsoleColor.White;
                    String yesorno = Console.ReadLine();
                    if (yesorno.Equals("y") || yesorno.Equals("Y"))
                    {
                        daDialog = selectFile(number);
                    }
                    else if (yesorno.Equals("n") || yesorno.Equals("N"))
                    {
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        static void initLogos()
        {
            logoimage yybLogoimage = new logoimage("应用宝大图标.png", 512, 200, 0);
            logoimage yybminLogoimage = new logoimage("应用宝小图标.png", 16, 20, 0);
            logoimage logoimage360 = new logoimage("360大图标.png", 512, 200, 70);
            logoimage huaweilogoimage = new logoimage("华为大图标.png", 216, 200, 70);
            logoimage lianxianglogoimage = new logoimage("联想大图标.png", 256, 200, 70);
            logoList.Add(yybLogoimage);
            logoList.Add(yybminLogoimage);
            logoList.Add(logoimage360);
            logoList.Add(huaweilogoimage);
            logoList.Add(lianxianglogoimage);
        }

        public static Bitmap RoundCorners(Bitmap StartImage, int CornerRadius, Color BackgroundColor)
        {
            CornerRadius *= 2;
            Bitmap RoundedImage = new Bitmap(StartImage.Width, StartImage.Height);
            using (Graphics g = Graphics.FromImage(RoundedImage))
            {
                g.Clear(BackgroundColor);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Brush brush = new TextureBrush(StartImage);
                GraphicsPath gp = new GraphicsPath();
                gp.AddArc(0, 0, CornerRadius, CornerRadius, 180, 90);
                gp.AddArc(0 + RoundedImage.Width - CornerRadius, 0, CornerRadius, CornerRadius, 270, 90);
                gp.AddArc(0 + RoundedImage.Width - CornerRadius, 0 + RoundedImage.Height - CornerRadius, CornerRadius,
                    CornerRadius, 0, 90);
                gp.AddArc(0, 0 + RoundedImage.Height - CornerRadius, CornerRadius, CornerRadius, 90, 90);
                g.FillPath(brush, gp);
                return RoundedImage;
            }
        }




        
    }


    class logoimage
    {
        public String name;
        public int size;
        public int maxSize;
        public int fall;

        public logoimage(String name, int size, int maxSize, int fail)
        {
            this.name = name;
            this.size = size;
            this.maxSize = maxSize;
            this.fall = fail;
        }
    }

    class advertising
    {
        public String name;
        public int height;
        public int width;
        public int maxSize;

        public advertising(String name, int height, int fail, int maxSize)
        {
            this.name = name;
            this.height = height;
            this.width = fail;
            this.maxSize = maxSize;
        }

        //创建 圆角图片的方法

        #region 方法参数的说明

        /*
         * sSrcFilePath 为原始图片路径
         * 
         * sDstFilePath  为创建完圆角的图片路径
         * 
         * sCornerLocation  使用 方法CreateRoundRectanglePath  返回GraphicsPath 类型 
         * 可选字符串为TopLeft,TopRight,BottomLeft,BottomRight
         */

        #endregion

        public static void CreateRoundedCorner(string sSrcFilePath, string sDstFilePath, string sCornerLocation)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(sSrcFilePath);
            Graphics g = Graphics.FromImage(image);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;
            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
            GraphicsPath rectPath = CreateRoundRectanglePath(rect, image.Width / 10, sCornerLocation); //构建圆角外部路径
            Brush b = new SolidBrush(Color.White); //圆角背景白色
            g.DrawPath(new Pen(b), rectPath);
            g.FillPath(b, rectPath);
            g.Dispose();
            image.Save(sDstFilePath, ImageFormat.Jpeg);
            image.Dispose();
        }


        private static GraphicsPath CreateRoundRectanglePath(Rectangle rect, int radius, string sPosition)
        {
            GraphicsPath rectPath = new GraphicsPath();
            switch (sPosition)
            {
                case "TopLeft":
                {
                    rectPath.AddArc(rect.Left, rect.Top, radius * 2, radius * 2, 180, 90);
                    rectPath.AddLine(rect.Left, rect.Top, rect.Left, rect.Top + radius);
                    break;
                }

                case "TopRight":
                {
                    rectPath.AddArc(rect.Right - radius * 2, rect.Top, radius * 2, radius * 2, 270, 90);
                    rectPath.AddLine(rect.Right, rect.Top, rect.Right - radius, rect.Top);
                    break;
                }

                case "BottomLeft":
                {
                    rectPath.AddArc(rect.Left, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
                    rectPath.AddLine(rect.Left, rect.Bottom - radius, rect.Left, rect.Bottom);
                    break;
                }

                case "BottomRight":
                {
                    rectPath.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
                    rectPath.AddLine(rect.Right - radius, rect.Bottom, rect.Right, rect.Bottom);
                    break;
                }
            }
            return rectPath;
        }
    }
}