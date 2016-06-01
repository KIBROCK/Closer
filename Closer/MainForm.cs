using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace closer
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}
		
		/// <summary>
		/// Импорты из gdi32.dll
		/// </summary>
		private class GDI32
        {
            public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter
            
            [DllImport("gdi32.dll")]
            public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hObjectSource, int nXSrc, int nYSrc, int dwRop);
            
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);
            
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
            
            [DllImport("gdi32.dll")]
            public static extern bool DeleteDC(IntPtr hDC);
            
            [DllImport("gdi32.dll")]
            public static extern bool DeleteObject(IntPtr hObject);
            
            [DllImport("gdi32.dll")]
            public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
            
            [DllImport("gdi32.dll")]
			public static extern bool LineTo(IntPtr hdc, int nXEnd, int nYEnd);
			
			[DllImport("gdi32.dll")]
			public static extern IntPtr CreateDIBSection(IntPtr hdc, [In] ref BITMAPINFO pbmi,uint iUsage, out IntPtr ppvBits, IntPtr hSection, uint dwOffset);
			
			[StructLayout(LayoutKind.Sequential)]
			public struct BITMAPINFO
			{
				public BITMAPINFOHEADER bmiHeader;
				public RGBQUAD bmiColors;
			}
			
			[StructLayout(LayoutKind.Sequential)]
			public struct BITMAPINFOHEADER
			{
				public UInt32 biSize;
				public Int32 biWidth;
				public Int32 biHeight;
				public Int16 biPlanes;
				public Int16 biBitCount;
				public UInt32 biCompression;
				public UInt32 biSizeImage;
				public Int32 biXPelsPerMeter;
				public Int32 biYPelsPerMeter;
				public UInt32 biClrUsed;
				public UInt32 biClrImportant;
			}
			
			[StructLayout(LayoutKind.Sequential)]
			public struct RGBQUAD
			{
				public byte rgbBlue;
				public byte rgbGreen;
				public byte rgbRed;
				public byte rgbReserved;
			}
			
			public static IntPtr CreateDIB(IntPtr hdc, int cx, int cy)
			{
				BITMAPINFO bmpInfo = new BITMAPINFO();
				bmpInfo.bmiHeader.biSize = 40;
				bmpInfo.bmiHeader.biWidth = cx;
				bmpInfo.bmiHeader.biHeight = cy;
				bmpInfo.bmiHeader.biPlanes = 1;
				bmpInfo.bmiHeader.biBitCount = 32;
				bmpInfo.bmiHeader.biCompression = 0;
				bmpInfo.bmiHeader.biSizeImage = 0;
				bmpInfo.bmiHeader.biXPelsPerMeter = 10000;
				bmpInfo.bmiHeader.biYPelsPerMeter = 10000;
				bmpInfo.bmiHeader.biClrUsed = 0;
				bmpInfo.bmiHeader.biClrImportant = 0;
				IntPtr pbytes = (IntPtr)null;
				return CreateDIBSection(hdc, ref bmpInfo, 0, out pbytes, IntPtr.Zero, 0);	
			}
        }

		/// <summary>
		/// Импотры из user32.dll
		/// </summary>
        private class User32
        {
        	public const uint WM_LBUTTONDOWN = 0x0201;
	        public const uint WM_LBUTTONUP = 0x0202;
	        public const uint WM_ACTIVATE = 0x0006;
	        public const int WM_SYSCOMMAND = 0x0112;
	        public const int SC_CLOSE = 0xF060;
        	
            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }

			[DllImport("user32.dll", EntryPoint = "GetDC")]
			public static extern IntPtr GetDC(IntPtr ptr);
            
            [DllImport("user32.dll")]
            public static extern IntPtr GetDesktopWindow();
            
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowDC(IntPtr hWnd);
            
            [DllImport("user32.dll")]
            public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
            
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
            
            [DllImport("user32.dll")]
            public static extern IntPtr GetClientRect(IntPtr hWnd, ref RECT rect);
            
            [DllImport("user32.DLL", CharSet = CharSet.Unicode)]
			public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
			
			[DllImport("user32.dll", SetLastError = true)]
	        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);
	        
	        [DllImport("user32.dll", SetLastError = true)]
	        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, int wParam, out string lParam);
	        
	        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
	        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string lclassName, string windowTitle);
	
	        [DllImport("user32.dll")]
	        public static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
	 
	        [DllImport("user32.dll")]
	        public static extern bool GetWindowRect(IntPtr handle, ref Rectangle rect);
	        
	        [DllImport("user32.dll")]
	        public static extern int GetWindowText(int hWnd, StringBuilder text, int count);
	        
	        [DllImport("user32.dll", SetLastError = true)]
        	public static extern int GetClassName ( IntPtr hWnd, StringBuilder lpString, int nMaxCount );
	        
	        public static string GetWindowText(IntPtr hWnd)
	        {
	            StringBuilder text = new StringBuilder(256);
	            if (GetWindowText(hWnd.ToInt32(), text, text.Capacity) > 0)
	            {
	                return text.ToString();
	            }
	            return String.Empty;
	        }
	        
	        public static string GetWindowClass (IntPtr hWnd) 
	        {
	            int len = 256;
	            StringBuilder sb = new StringBuilder(len);
	            len = GetClassName(hWnd, sb, len);
	            return sb.ToString(0, len);
	        }
        }
		
        List<string> lines = new List<string>(); // Список строк из настроек
				
		void MainFormLoad(object sender, EventArgs e)
		{
			notifyIcon1.Text += " " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
			this.Hide();
			Setting();
		}
		
		/// <summary>
		/// Заполнение списка из файла настроек
		/// </summary>
		void Setting() 
		{
			if (File.Exists(@"Settings.txt")) // Если файл с настройками есть
            {
            	try
            	{
            		lines.Clear(); // Очищаем список
	                string[] strings = File.ReadAllLines(@"Settings.txt", Encoding.GetEncoding(1251)); // Читаем все строки из файла
	                foreach (string lin in strings) // По каждой строке
	                {
	                	if ((lin != "") && (lin.IndexOf("%") != 0))  // Если не пустая и не коммент
	                	{
	                		lines.Add(lin); // Добавляем в список
	                	}
	                }
	                if ((lines.Count - 3) % 2 != 0) // Если кол-во не пустых строк не четное, информируем и выходим
	                {
	                	MessageBox.Show("В файле настроек кол-во значений не четное", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            			Application.Exit();
	                }
	                if (timer1.Interval != Convert.ToInt32(lines[2])) timer1.Interval = Convert.ToInt32(lines[4]);
            	}
            	catch (Exception ex)
            	{
            		MessageBox.Show("Какой-то трабл при чтении файла настроек: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            		Application.Exit();
            	}
            }
            else // Если файл не найден, информируем и выходим
            {
                MessageBox.Show("Не найден файл с настройками (Settings.txt)", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
		}
		
		void MenuExitClick(object sender, EventArgs e)
		{
			Application.Exit();
		}
		
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			notifyIcon1.Visible = false;
		}
		
		void Timer1Tick(object sender, EventArgs e)
		{
			try
			{
				if (!contextMenuStrip1.Visible) // Если открыто меню то не работает (чтобы не мешало)
				{
					if ((lines.Count - 3) % 2 == 0) // Если кол-во строк из настроек четное
					{
						for (int i = 3; i < lines.Count; i+=2) // Начиная с 3 берем по парам
		                {
							if (lines[i].Contains("[n]"))
							{
								lines[i] = lines[i].Substring(0, lines[i].IndexOf("[n]")) + "\n" + lines[i].Substring(lines[i].IndexOf("[n]")+3);
							}
							var handw = User32.FindWindow(null, lines[i]); // Хендл окна
							if (handw != (IntPtr)0x0)
							{
								Image img = CaptureWindow(handw, 0, 0, 0, 0); // Получаем картинку
								String title = User32.GetWindowText(handw);
								if (lines[i+1] == "!exit!") // Если "!exit!" то просто гасим окно
								{
									User32.SendMessage(handw, User32.WM_SYSCOMMAND, User32.SC_CLOSE, 0); // Гасим
									if (handw != (IntPtr)0x0) // Если загасили сообщаем
						            {
										GetScreen(img, title);
										notifyIcon1.ShowBalloonTip(500, "Закрыто приложение", lines[i], ToolTipIcon.Info );
									}
								}
								else
								{ // Если указана конкретная кнопка
									var hand = User32.FindWindowEx(User32.FindWindow(null, lines[i]), (IntPtr)0, "Button", lines[i+1]); // Берем её хендл
						            User32.PostMessage(hand, User32.WM_ACTIVATE, 1, 0); // и пытаемся нажать
						            User32.PostMessage(hand, User32.WM_LBUTTONDOWN, 0, 0);
						            User32.PostMessage(hand, User32.WM_LBUTTONUP, 0, 0);
						            if (hand != (IntPtr)0x0) // Если нажали сообщаем
						            {
						            	GetScreen(img, title);
						            	notifyIcon1.ShowBalloonTip(500, lines[i+1], lines[i], ToolTipIcon.Info );
						            }
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				notifyIcon1.ShowBalloonTip(1000, "Ошибка при попытке нажатия кнопки", ex.Message, ToolTipIcon.Error);
			}
		}
		
		/// <summary>
		/// Берем картинку
		/// </summary>
		/// <param name="path">папка куда сохранять картинку</param>
		/// <param name="name">имя файла картинки</param>
		public void GetScreen(Image img, string title)
		{
			try
			{
				Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(delegate {return false;}); // Калбэк для эскиза (пустой, просто нужен)
				Bitmap bm = new Bitmap(img.GetThumbnailImage(50, 50, myCallback, IntPtr.Zero)); // Делаем эскиз 50х50 для быстрой обработки
				bool next = false; // Флаг сохранения картинки
				for (int i = 0; i < bm.Width; i++) { // Бежим по всем пикселям эскиза
					for (int j = i; j < bm.Height; j++) {
						if (bm.GetPixel(i, j).Name != "ff000000") // Если хоть один пиксель не черный 
						{
							next = true; // Можно сохранять
						}
					}
				}
				
				string path = lines[0]; // Путь
				string name = lines[1]; // Имя
				if (path.IndexOf("!comp!") >= 0) // Если в пути есть "!comp!" 
				{
					path = path.Replace("!comp!", SystemInformation.ComputerName); // Меняем на имя компа
				}
				if (name == "!comp!") // Если "!comp!" назовем файл по имени компа
				{
					name = SystemInformation.ComputerName;
				}
				if (name == "!title!") // Если "!title!" назовем файл по имени окна
				{
					name = title;
				}
				if (!Directory.Exists(path)) // Если директории для сохранения нет
				{
					Directory.CreateDirectory(path); // Создаем её
				}
				if (next) // Если можно сохранять
				{
					if (File.Exists(path + "\\" + name + ".jpg"))
					{
						int i = 0;
						while (File.Exists(path + "\\" + name + i.ToString() + ".jpg")) { i++; }
						name += i.ToString();
					}
					img.Save(path + "\\" + name + ".jpg", ImageFormat.Jpeg);
				}
			}
			catch (Exception ex)
			{
				notifyIcon1.ShowBalloonTip(1000, "Ошибка при попытке снять картинку", ex.Message, ToolTipIcon.Error);
			}
		}
		
		/// <summary>
		/// Захват картинки с экрана
		/// </summary>
		/// <param name="handle">хендл по которому захватывать изображение</param>
		/// <param name="X">обрезка картинки слева</param>
		/// <param name="Y">обрезка картинки сверху</param>
		/// <param name="XX">обрезка картинки справа</param>
		/// <param name="YY">обрезка картинки снизу</param>
		/// <returns></returns>
		public Image CaptureWindow(IntPtr handle, int X, int Y, int XX, int YY)
        {
			try
			{
	            IntPtr hdcSrc = User32.GetWindowDC(handle); // Берем контекст дисплея
	            User32.RECT windowRect = new User32.RECT(); // Берем треугольник захвата
	            User32.GetWindowRect(handle, ref windowRect); // Задаем ему размеры по хендлу
	           	windowRect.left += X; // Обрезаем слева
	           	windowRect.top += Y; // Обрезаем сверху
	           	windowRect.right += XX; // Обрезаем справа
	           	windowRect.bottom += YY; // Обрезаем снизу
	            int width = windowRect.right - windowRect.left; // Задаем ширину
	            int height = windowRect.bottom - windowRect.top; // Задаем высоту
	            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc); // Создаем контекст устpойства памяти
	            IntPtr hBitmap = GDI32.CreateDIB(hdcSrc, width, height); // Создаем битмап
	            Image img = null;
	            if (hBitmap != IntPtr.Zero)
	            {
	            	IntPtr hOld = GDI32.SelectObject(hdcDest, hBitmap); // Совмещаем
	            	GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, X, Y, GDI32.SRCCOPY); // Копиpуем битмап
	            	GDI32.SelectObject(hdcDest, hOld);
	            	GDI32.DeleteDC(hdcDest); //Удаляем контекст устpойства
	            	User32.ReleaseDC(handle, hdcSrc); // Освобождаем контекст устpойства
	            	img = Image.FromHbitmap(hBitmap); // Делаем Image из битмапа
	            }
	            GDI32.DeleteObject(hBitmap); // Гасим битмап
	            return img;
	        }
			catch (Exception ex)
			{
				notifyIcon1.ShowBalloonTip(1000, "Ошибка при захвате экрана handl: " + handle.ToString() + " Error: ", ex.Message, ToolTipIcon.Error);
				return null;
			}
        }
		
		void MenuSettingsClick(object sender, EventArgs e)
		{
			if (File.Exists(@"Settings.txt")) // Если файл с настройками есть
            {
				Process.Start("Settings.txt").WaitForExit(); // Открываем его в блокноте и ждем закрытия 
				Setting(); // А когда закроется перечитываем новые настройки
            }
            else // Если файла с настройками нет, информиркем и выходим (работать без них все равно не будет) 
            {
                MessageBox.Show("Не найден файл с настройками (Settings.txt)", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
		}
	}
}
