using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Xml.Serialization;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Lab2GK.Model;
using Microsoft.Win32;

namespace Lab2GK.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private bool _czyStalyKolor;
        private bool _nVectorStaly;
        private FillingMode _fillingColorMode;
        private bool _kandMRandom;
        private double _kdValue;
        private double _ksValue;
        private double _mValue;
        private bool _isLightVectorConstant;
        private WriteableBitmap _bitmap;
        private int _ntr;
        private int _mtr;
        private (int x, int y)[,] _points;
        private ObservableCollection<(int x1, int y1, int x2, int y2, int x3, int y3)> _triangles;
        private WriteableBitmap _originalBitmap;
        private WriteableBitmap _normalBitmap;
        private int _defR;
        private int _defG;
        private int _defB;
        private WriteableBitmap _pictureBitmap;
        private bool _isBusy;
        private (double kd, double ks, double m)[] _randTab;
        private bool _movingTrianglesMode;
        private bool _isVertexCaptured;
        private (int indX, int indY) _capturedVertexInd;
        private BitmapImage _normalBitmapImage;
        private BitmapImage _imageBitmapImage;
        private Color _selectedColor;
        private Color _selectedLightColor;
        private bool _isSpotlightOn;
        private int _spotlightHeight;
        private double _SpotlightFocus;
        private bool _showTriangles;

        public WriteableBitmap Bitmap
        {
            get { return _bitmap; }
            set { _bitmap = value; RaisePropertyChanged(nameof(Bitmap)); }
        }

        public WriteableBitmap OriginalBitmap
        {
            get { return _originalBitmap; }
            set { _originalBitmap = value; RaisePropertyChanged(nameof(OriginalBitmap)); }
        }

        public WriteableBitmap NormalBitmap
        {
            get { return _normalBitmap; }
            set { _normalBitmap = value; RaisePropertyChanged(nameof(NormalBitmap)); }
        }

        public (double kd, double ks, double m)[] RandTab
        {
            get { return _randTab; }
            set { _randTab = value; RaisePropertyChanged(nameof(RandTab)); }
        }

        public (byte r, byte g, byte b)[] NormalColors
        {
            get; set;
        }

        public WriteableBitmap PictureBitmap
        {
            get { return _pictureBitmap; }
            set { _pictureBitmap = value; RaisePropertyChanged(nameof(PictureBitmap)); }
        }

        public BitmapImage NormalBitmapImage
        {
            get { return _normalBitmapImage; }
            set { _normalBitmapImage = value; RaisePropertyChanged(nameof(NormalBitmapImage)); }
        }

        public BitmapImage ImageBitmapImage
        {
            get { return _imageBitmapImage; }
            set { _imageBitmapImage = value; RaisePropertyChanged(nameof(ImageBitmapImage)); }
        }

        public (byte r, byte g, byte b)[] PictureColors
        {
            get; set;
        }

        public int Ntr
        {
            get { return _ntr; }
            set { _ntr = value; RaisePropertyChanged(nameof(Ntr)); }
        }

        public int Mtr
        {
            get { return _mtr; }
            set { _mtr = value; RaisePropertyChanged(nameof(Mtr)); }
        }

        public (int x, int y)[,] Points
        {
            get { return _points; }
            set { _points = value; RaisePropertyChanged(nameof(Points)); }
        }

        public ObservableCollection<(int x1, int y1, int x2, int y2, int x3, int y3)> Triangles
        {
            get { return _triangles; }
            set { _triangles = value; RaisePropertyChanged(nameof(Triangles)); }
        }

        public bool CzyStalyKolor
        {
            get { return _czyStalyKolor; }
            set { _czyStalyKolor = value; RaisePropertyChanged(nameof(CzyStalyKolor)); }
        }

        public bool NVectorStaly
        {
            get { return _nVectorStaly; }
            set { _nVectorStaly = value; RaisePropertyChanged(nameof(NVectorStaly)); }
        }

        public bool IsSpotlightOn
        {
            get { return _isSpotlightOn; }
            set { _isSpotlightOn = value; RaisePropertyChanged(nameof(IsSpotlightOn)); }
        }

        public double KdValue
        {
            get { return _kdValue; }
            set { _kdValue = value; RaisePropertyChanged(nameof(KdValue)); }
        }

        public double KsValue
        {
            get { return _ksValue; }
            set { _ksValue = value; RaisePropertyChanged(nameof(KsValue)); }
        }

        public double MValue
        {
            get { return _mValue; }
            set { _mValue = value; RaisePropertyChanged(nameof(MValue)); }
        }

        public int SpotlightHeight
        {
            get { return _spotlightHeight; }
            set { _spotlightHeight = value; RaisePropertyChanged(nameof(SpotlightHeight)); }
        }

        public double SpotlightFocus
        {
            get { return _SpotlightFocus; }
            set { _SpotlightFocus = value; RaisePropertyChanged(nameof(SpotlightFocus)); }
        }

        public int DefR
        {
            get { return SelectedColor.R; }
        }

        public int DefG
        {
            get { return SelectedColor.G; }
        }

        public int DefB
        {
            get { return SelectedColor.B; }
        }

        public (int x, int y, int z) SpotR
        {
            get { return (0, 0, SpotlightHeight); }
        }

        public (int x, int y, int z) SpotG
        {
            get { return (Bitmap.PixelWidth, 0, SpotlightHeight); }
        }

        public (int x, int y, int z) SpotB
        {
            get { return (Bitmap.PixelWidth / 2, Bitmap.PixelHeight, SpotlightHeight); }
        }

        public int LightR { get { return SelectedLightColor.R; } }

        public int LightG { get { return SelectedLightColor.G; } }

        public int LightB { get { return SelectedLightColor.B; } }


        public FillingMode FillingColorMode
        {
            get { return _fillingColorMode; }
            set { _fillingColorMode = value; RaisePropertyChanged(nameof(FillingColorMode)); }
        }

        public bool KandMRandom
        {
            get { return _kandMRandom; }
            set { _kandMRandom = value; RaisePropertyChanged(nameof(KandMRandom)); }
        }

        public bool IsLightVectorConstant
        {
            get { return _isLightVectorConstant; }
            set { _isLightVectorConstant = value; RaisePropertyChanged(nameof(IsLightVectorConstant));}
        }

        public bool IsBusy
        {
            get => _isBusy;
            private set => Set(ref _isBusy, value);
        }

        public bool MovingTrianglesMode
        {
            get { return _movingTrianglesMode; }
            set { _movingTrianglesMode = value; RaisePropertyChanged(nameof(MovingTrianglesMode)); }
        }

        public bool ShowTriangles
        {
            get { return _showTriangles; }
            set { _showTriangles = value; RaisePropertyChanged(nameof(ShowTriangles)); }
        }

        public bool IsVertexCaptured
        {
            get { return _isVertexCaptured; }
            set { _isVertexCaptured = value; RaisePropertyChanged(nameof(IsVertexCaptured)); }
        }

        public (int indX, int indY) CapturedVertexInd
        {
            get { return _capturedVertexInd; }
            set { _capturedVertexInd = value; RaisePropertyChanged(nameof(CapturedVertexInd)); }
        }

        public Color SelectedColor
        {
            get { return _selectedColor; }
            set { _selectedColor = value; RaisePropertyChanged(nameof(SelectedColor));
                RaisePropertyChanged(nameof(DefR));
                RaisePropertyChanged(nameof(DefG));
                RaisePropertyChanged(nameof(DefB));
            }
        }

        public Color SelectedLightColor
        {
            get { return _selectedLightColor; }
            set
            {
                _selectedLightColor = value; RaisePropertyChanged(nameof(SelectedLightColor));
                RaisePropertyChanged(nameof(LightR));
                RaisePropertyChanged(nameof(LightG));
                RaisePropertyChanged(nameof(LightB));
            }
        }

        public RelayCommand<string> ObjectColorRadioButtonCommand { get; private set; }

        public RelayCommand<string> NVectorRadioButtonCommand { get; private set; }

        public RelayCommand<string> FillingColorRadioButtonCommand { get; private set; }

        public RelayCommand<string> KandMRadioButtonCommand { get; private set; }

        public RelayCommand<string> LightVectorRadioButtonCommand { get; private set; }

        public RelayCommand LoadNormalCommand { get; private set; }

        public RelayCommand DoMagicCommand { get; private set; }

        public RelayCommand CLICKCommand { get; private set; }

        public RelayCommand LoadImageCommand { get; private set; }

        public AsyncCommand AsyncCmd { get; private set; }
        public RelayCommand<(int x, int y)> MouseDownOnBitmap
        {
            get;
            private set;
        }

        public MainViewModel()
        {
            MValue = 1.0;
            KdValue = 0.5;
            KsValue = 0.5;
            SpotlightFocus = 50;
            SpotlightHeight = 30;

            CzyStalyKolor = false;
            NVectorStaly = false;
            FillingColorMode = FillingMode.Exact;
            KandMRandom = false;
            IsLightVectorConstant = true;
            IsSpotlightOn = false;

            ShowTriangles = true;

            SelectedColor = Colors.White;
            SelectedLightColor = Colors.White;

            Mtr = 10;
            Ntr = 10;

            Points = new (int x, int y)[Mtr + 1, Ntr + 1];
            Triangles = new ObservableCollection<(int x1, int y1, int x2, int y2, int x3, int y3)>();

            LoadDefaultNormal("../../NormalMaps/downloadxd.png");
            LoadDefaultImage("../../NormalMaps/download (2).png");

            ObjectColorRadioButtonCommand = new RelayCommand<string>(ObjectColorRadioButtonClick);
            NVectorRadioButtonCommand = new RelayCommand<string>(NVectorRadioButtonClicked);
            FillingColorRadioButtonCommand = new RelayCommand<string>(FillingColorRadioButtonClick);
            KandMRadioButtonCommand = new RelayCommand<string>(KandMRadioButtonClick);
            LightVectorRadioButtonCommand = new RelayCommand<string>(LightVectorRadioButtonClick);
            LoadNormalCommand = new RelayCommand(LoadNormal);
            CLICKCommand = new RelayCommand(ApplyChanges);
            LoadImageCommand = new RelayCommand(LoadImage);
            AsyncCmd = new AsyncCommand(Animation, CanExecuteAnimation);
            MouseDownOnBitmap = new RelayCommand<(int x, int y)>(BitmapDownClick);
        }

        private void BitmapDownClick((int x, int y) p)
        {
            if (MovingTrianglesMode)
            {
                if (!IsVertexCaptured)
                {
                    var v = FindPoint(p.x, p.y);
                    if (v.indX != -1)
                    {
                        IsVertexCaptured = true;
                        CapturedVertexInd = v;
                    }
                }
                else
                {
                    Points[CapturedVertexInd.indX, CapturedVertexInd.indY] = p;
                    IsVertexCaptured = false;
                    if (ImageBitmapImage != null)
                    {
                        Bitmap = new WriteableBitmap(ImageBitmapImage);
                    }
                    else if (NormalBitmapImage != null)
                    {
                        Bitmap = new WriteableBitmap(NormalBitmapImage);
                    }
                    RaisePropertyChanged(nameof(Bitmap));
                    SetTriangles();
                    ApplyChanges();
                }
            }
        }

        private (int indX, int indY) FindPoint(int x, int y)
        {
            for (int i = 1; i < Mtr; i++)
            {
                for (int j = 1; j < Ntr; j++)
                {
                    if (x >= Points[i, j].x - 2 && x <= Points[i, j].x + 2 && y <= Points[i, j].y + 2 &&
                        y >= Points[i, j].y - 2)
                    {
                        return (i, j);
                    }
                }
            }

            return (-1, -1);
        }

        private async Task Animation()
        {
            if (!IsLightVectorConstant && !IsSpotlightOn)
            {
                try
                {
                    IsBusy = true;
                    Task<int> xd = AnimationAsync();
                    int result = await xd;
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        public async Task<int> AnimationAsync()
        {
            int radius = 2000;
            (int x, int y) lightSource;
            (int x, int y) origin = (0, 0);
            var rand = new Random();


            for (int j = 0; j <= 360; j += 5)
            {
                lightSource = CirclePoint(radius, j, origin);

                for (int i = 0; i < Triangles.Count; i++)
                    FillPolygon(i, rand, lightSource.x, lightSource.y, (Triangles[i].x1, Triangles[i].y1), (Triangles[i].x2, Triangles[i].y2), (Triangles[i].x3, Triangles[i].y3));
                DrawTriangles();


                await Task.Delay(1);
            }

            return 1;
        }

        private bool CanExecuteAnimation()
        {
            return !IsBusy;
        }

        private void LoadDefaultImage(string path)
        {
            Uri fileUri = new Uri(path, UriKind.Relative);
            ImageBitmapImage = new BitmapImage(fileUri);

            PictureBitmap = new WriteableBitmap(ImageBitmapImage);
            Bitmap = new WriteableBitmap(ImageBitmapImage);

            PictureColors = new (byte r, byte g, byte b)[PictureBitmap.PixelWidth * PictureBitmap.PixelHeight];

            unsafe
            {
                using (var context = PictureBitmap.GetBitmapContext(ReadWriteMode.ReadOnly))
                {
                    for (int i = 0; i < PictureColors.Length; i++)
                    {
                        var c = context.Pixels[i];
                        var a = (byte)(c >> 24);
                        int ai = a;
                        if (ai == 0)
                        {
                            ai = 1;
                        }
                        ai = ((255 << 8) / ai);
                        PictureColors[i] = ((byte)((((c >> 16) & 0xFF) * ai) >> 8), (byte)((((c >> 8) & 0xFF) * ai) >> 8), (byte)((((c & 0xFF) * ai) >> 8)));
                    }
                }
            }

            Points = new (int x, int y)[Mtr + 1, Ntr + 1];
            Triangles = new ObservableCollection<(int x1, int y1, int x2, int y2, int x3, int y3)>();

            SetPoints();
            SetTriangles();
            DrawTriangles();
        }

        private void LoadImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Uri fileUri = new Uri(openFileDialog.FileName);
                ImageBitmapImage = new BitmapImage(fileUri);

                PictureBitmap = new WriteableBitmap(ImageBitmapImage);
                Bitmap = new WriteableBitmap(ImageBitmapImage);

                PictureColors = new (byte r, byte g, byte b)[PictureBitmap.PixelWidth * PictureBitmap.PixelHeight];

                if (NormalBitmap == null || NormalBitmap.PixelHeight != PictureBitmap.PixelHeight ||
                    NormalBitmap.PixelWidth != PictureBitmap.PixelWidth)
                {
                    NormalBitmap = new WriteableBitmap(ImageBitmapImage);

                    NormalColors = new (byte r, byte g, byte b)[NormalBitmap.PixelWidth * NormalBitmap.PixelHeight];

                    unsafe
                    {
                        using (var context = NormalBitmap.GetBitmapContext(ReadWriteMode.ReadOnly))
                        {
                            for (int i = 0; i < NormalColors.Length; i++)
                            {
                                var c = context.Pixels[i];
                                var a = (byte)(c >> 24);
                                int ai = a;
                                if (ai == 0)
                                {
                                    ai = 1;
                                }
                                ai = ((255 << 8) / ai);
                                NormalColors[i] = ((byte)((((c >> 16) & 0xFF) * ai) >> 8), (byte)((((c >> 8) & 0xFF) * ai) >> 8), (byte)((((c & 0xFF) * ai) >> 8)));
                            }
                        }
                    }
                }

                unsafe
                {
                    using (var context = PictureBitmap.GetBitmapContext(ReadWriteMode.ReadOnly))
                    {
                        for (int i = 0; i < PictureColors.Length; i++)
                        {
                            var c = context.Pixels[i];
                            var a = (byte)(c >> 24);
                            int ai = a;
                            if (ai == 0)
                            {
                                ai = 1;
                            }
                            ai = ((255 << 8) / ai);
                            PictureColors[i] = ((byte)((((c >> 16) & 0xFF) * ai) >> 8), (byte)((((c >> 8) & 0xFF) * ai) >> 8), (byte)((((c & 0xFF) * ai) >> 8)));
                        }
                    }
                }

                Points = new (int x, int y)[Mtr + 1, Ntr + 1];
                Triangles = new ObservableCollection<(int x1, int y1, int x2, int y2, int x3, int y3)>();

                SetPoints();
                SetTriangles();
                DrawTriangles();
            }
        }

        private void SetPoints()
        {
            var xs = new int[Mtr + 1];
            var ys = new int[Ntr + 1];

            xs[0] = 0;
            ys[0] = 0;

            for (int i = 1; i < xs.Length - 1; i++)
            {
                xs[i] = xs[i - 1] + (Bitmap.PixelWidth - xs[i - 1]) / (Mtr - i + 1);
            }

            xs[xs.Length - 1] = Bitmap.PixelWidth - 1;

            for (int i = 1; i < ys.Length - 1; i++)
            {
                ys[i] = ys[i - 1] + (Bitmap.PixelWidth - ys[i - 1]) / (Ntr - i + 1);
            }

            ys[ys.Length - 1] = Bitmap.PixelWidth - 1;

            for (int i = 0; i < Mtr + 1; i++)
            {
                for (int j = 0; j < Ntr + 1; j++)
                {
                    Points[i, j] = (xs[i], ys[j]);
                }
            }
        }

        private void SetTriangles()
        {
            Triangles = new ObservableCollection<(int x1, int y1, int x2, int y2, int x3, int y3)>();
            for (int i = 0; i < Mtr; i++)
            {
                for (int j = 0; j < Ntr; j++)
                {
                    Triangles.Add((Points[i, j].x, Points[i, j].y, Points[i + 1, j].x, Points[i + 1, j].y,
                        Points[i, j + 1].x, Points[i, j + 1].y));

                    Triangles.Add((Points[i + 1, j + 1].x, Points[i + 1, j + 1].y, Points[i, j + 1].x,
                        Points[i, j + 1].y, Points[i + 1, j].x, Points[i + 1, j].y));
                }
            }
        }

        private void DrawTriangles()
        {
            if (ShowTriangles)
            {
                foreach (var t in Triangles)
                {
                    Bitmap.DrawTriangle(t.x1, t.y1, t.x2, t.y2, t.x3, t.y3, Colors.Black);
                }
            }
        }

        private void ObjectColorRadioButtonClick(string name)
        {
            CzyStalyKolor = name == "objectColorRadioButton1";
        }
        private void NVectorRadioButtonClicked(string name)
        {
            NVectorStaly = name == "nVectorRadioButton1";
        }

        private void FillingColorRadioButtonClick(string name)
        {
            if (name == "fillingColorRadioButton1")
                FillingColorMode = FillingMode.Exact;
            else if (name == "fillingColorRadioButton2")
                FillingColorMode = FillingMode.Interpolated;
            else
                FillingColorMode = FillingMode.Hybrid;
        }

        private void KandMRadioButtonClick(string name)
        {
            KandMRandom = name == "KandMRadioButton2";
            if (KandMRandom)
            {
                Random rand = new Random();

                    RandTab = new (double kd, double ks, double m)[Triangles.Count];
                    for(int i = 0; i < RandTab.Length; i++)
                    {
                        double ks = rand.NextDouble();
                        double kd = rand.NextDouble();
                        double m = (double)rand.Next(101);

                        RandTab[i] = (kd, ks, m);
                    }
            }
        }

        private void LightVectorRadioButtonClick(string name)
        {
            if (name == "lightVectorRadioButton1")
            {
                IsLightVectorConstant = true;
                IsSpotlightOn = false;
            }
            else if (name == "lightVectorRadioButton2")
            {
                IsLightVectorConstant = false;
                IsSpotlightOn = false;
            }
            else
            {
                IsLightVectorConstant = false;
                IsSpotlightOn = true;
            }
        }

        private void LoadDefaultNormal(string path)
        {
            Uri fileUri = new Uri(path, UriKind.Relative);
            NormalBitmapImage = new BitmapImage(fileUri);

            NormalBitmap = new WriteableBitmap(NormalBitmapImage);
            Bitmap = new WriteableBitmap(NormalBitmapImage);

            NormalColors = new (byte r, byte g, byte b)[NormalBitmap.PixelWidth * NormalBitmap.PixelHeight];

            unsafe
            {
                using (var context = NormalBitmap.GetBitmapContext(ReadWriteMode.ReadOnly))
                {
                    for (int i = 0; i < NormalColors.Length; i++)
                    {
                        var c = context.Pixels[i];
                        var a = (byte)(c >> 24);
                        int ai = a;
                        if (ai == 0)
                        {
                            ai = 1;
                        }
                        ai = ((255 << 8) / ai);
                        NormalColors[i] = ((byte)((((c >> 16) & 0xFF) * ai) >> 8), (byte)((((c >> 8) & 0xFF) * ai) >> 8), (byte)((((c & 0xFF) * ai) >> 8)));
                    }
                }
            }

            Points = new (int x, int y)[Mtr + 1, Ntr + 1];
            Triangles = new ObservableCollection<(int x1, int y1, int x2, int y2, int x3, int y3)>();

            SetPoints();
            SetTriangles();
            DrawTriangles();
        }

        private void LoadNormal()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Uri fileUri = new Uri(openFileDialog.FileName);
                NormalBitmapImage = new BitmapImage(fileUri);

                NormalBitmap = new WriteableBitmap(NormalBitmapImage);
                Bitmap = new WriteableBitmap(NormalBitmapImage);

                NormalColors = new (byte r, byte g, byte b)[NormalBitmap.PixelWidth * NormalBitmap.PixelHeight];

                if (PictureBitmap == null || NormalBitmap.PixelHeight != PictureBitmap.PixelHeight ||
                    NormalBitmap.PixelWidth != PictureBitmap.PixelWidth)
                {
                    PictureBitmap = new WriteableBitmap(NormalBitmapImage);

                    PictureColors = new (byte r, byte g, byte b)[PictureBitmap.PixelWidth * PictureBitmap.PixelHeight];

                    unsafe
                    {
                        using (var context = PictureBitmap.GetBitmapContext(ReadWriteMode.ReadOnly))
                        {
                            for (int i = 0; i < PictureColors.Length; i++)
                            {
                                var c = context.Pixels[i];
                                var a = (byte)(c >> 24);
                                int ai = a;
                                if (ai == 0)
                                {
                                    ai = 1;
                                }
                                ai = ((255 << 8) / ai);
                                PictureColors[i] = ((byte)((((c >> 16) & 0xFF) * ai) >> 8), (byte)((((c >> 8) & 0xFF) * ai) >> 8), (byte)((((c & 0xFF) * ai) >> 8)));
                            }
                        }
                    }
                }

                unsafe
                {
                    using(var context = NormalBitmap.GetBitmapContext(ReadWriteMode.ReadOnly))
                    {
                        for (int i = 0; i < NormalColors.Length; i++)
                        {
                            var c = context.Pixels[i];
                            var a = (byte)(c >> 24);
                            int ai = a;
                            if (ai == 0)
                            {
                                ai = 1;
                            }
                            ai = ((255 << 8) / ai);
                            NormalColors[i] = ((byte)((((c >> 16) & 0xFF) * ai) >> 8), (byte)((((c >> 8) & 0xFF) * ai) >> 8), (byte)((((c & 0xFF) * ai) >> 8)));
                        }
                    }
                }

                Points = new (int x, int y)[Mtr + 1, Ntr + 1];
                Triangles = new ObservableCollection<(int x1, int y1, int x2, int y2, int x3, int y3)>();

                SetPoints();
                SetTriangles();
                DrawTriangles();
            }
        }

        private void ApplyChanges()
        {
            var rand = new Random();
            for(int i = 0; i < Triangles.Count; i++)
                FillPolygon(i, rand, 0, 0, (Triangles[i].x1, Triangles[i].y1), (Triangles[i].x2, Triangles[i].y2), (Triangles[i].x3, Triangles[i].y3));
            DrawTriangles();
        }

        private double Cos((double x, double y, double z) v1, (double x, double y, double z) v2)
        {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }



        private void FillPolygon(int poliInd, Random rand, int lightX = 0, int lightY = 0, params (int x, int y)[] P)
        {
            var colorsList = new List<(int ind, byte r, byte g, byte b)>();
            var wdth = NormalBitmap != null ? NormalBitmap.PixelWidth : PictureBitmap.PixelWidth;
            double kd = KdValue;
            double ks = KsValue;
            double m = MValue;

            if (KandMRandom)
            {
                kd = RandTab[poliInd].kd;
                ks = RandTab[poliInd].ks;
                m = RandTab[poliInd].m;
            }

            var ind = new int[P.Length];
            var Plist = P.ToList();

            for (int i = 0; i < ind.Length; i++)
            {
                var ymini = Plist[0].y;
                var indmin = 0;
                for (int j = 0; j < Plist.Count; j++)
                {
                    if (Plist[j].y < ymini)
                    {
                        ymini = Plist[j].y;
                        indmin = j;
                    }
                }

                ind[i] = indmin;
                Plist[indmin] = (Plist[indmin].x, Int32.MaxValue);
            }

            var ymin = P[ind[0]].y;
            var ymax = P[ind[ind.Length - 1]].y;
            var AET = new List<((int x, int y) v1, (int x, int y) v2)>();
            var prevScanV = new List<(int x, int y)>();
            Plist = P.ToList();
            for (int scan = ymin; scan < ymax; scan++)
            {
                prevScanV = new List<(int x, int y)>();

                for (int i = 0; i < Plist.Count; i++)
                {
                    if (Plist[i].y == scan)
                    {
                        prevScanV.Add(Plist[i]);
                    }
                }

                foreach (var v in prevScanV)
                {
                    var prevInd = (Plist.IndexOf(v) - 1 + Plist.Count) % Plist.Count;
                    var vprev = Plist[prevInd];
                    if (vprev.y >= v.y)
                    {
                        AET.Add((vprev, v));
                        if (v.y == vprev.y)
                            AET.Remove((vprev, v));
                    }
                    else
                    {
                        AET.Remove((vprev, v));
                    }

                    var vnext = Plist[(Plist.IndexOf(v) + 1 + Plist.Count) % Plist.Count];
                    if (vnext.y >= v.y)
                    {
                        AET.Add((v, vnext));
                        if (v.y == vnext.y)
                            AET.Remove((v, vnext));
                    }
                    else
                    {
                        AET.Remove((v, vnext));
                    }

                    AET.Sort((e1, e2) => { return GetX(e1.v1, e1.v2, scan) - GetX(e2.v1, e2.v2, scan);});
                }


                for (int i = 0; i < AET.Count / 2; i++)
                {
                    //wypelnianie
                    for (int j = GetX(AET[2 * i].v1, AET[2 * i].v2, scan);
                        j <= GetX(AET[2 * i + 1].v1, AET[2 * i + 1].v2, scan);
                        j++)
                    {

                        (int r, int g, int b) col = (0, 0, 0);
                        (double x, double y, double z) lightVec = (0, 0, 1);

                        if (!IsLightVectorConstant)
                        {
                            lightVec = (lightX - j, lightY - scan, 1420);
                            lightVec = Normalize(lightVec);
                        }

                        if (IsSpotlightOn)
                        {

                        }

                        var p0col = CalculateColorExact(P[0].x, P[0].y, P[0], P[1], P[2], kd, ks, m, lightVec.x, lightVec.y, lightVec.z);
                        var p1col = CalculateColorExact(P[1].x, P[1].y, P[0], P[1], P[2], kd, ks, m, lightVec.x, lightVec.y, lightVec.z);
                        var p2col = CalculateColorExact(P[2].x, P[2].y, P[0], P[1], P[2], kd, ks, m, lightVec.x, lightVec.y, lightVec.z);

                        if (FillingColorMode == FillingMode.Exact)
                            col = CalculateColorExact(j, scan, P[0], P[1], P[2], kd, ks, m, lightVec.x, lightVec.y, lightVec.z);
                        else if (FillingColorMode == FillingMode.Interpolated)
                        {
                            double p0dist = Distance(j, scan, P[0].x, P[0].y);
                            double p1dist = Distance(j, scan, P[1].x, P[1].y);
                            double p2dist = Distance(j, scan, P[2].x, P[2].y);

                            col = CalculateColorInterpolated(j, scan, p0col, p1col, p2col, kd, ks, m,
                                p0dist, p1dist, p2dist);
                        }
                        else
                        {
                            (double x, double y, double z) p0N = (0, 0, 1);
                            (double x, double y, double z) p1N = (0, 0, 1);
                            (double x, double y, double z) p2N = (0, 0, 1);

                            double p0dist = Distance(j, scan, P[0].x, P[0].y);
                            double p1dist = Distance(j, scan, P[1].x, P[1].y);
                            double p2dist = Distance(j, scan, P[2].x, P[2].y);

                            if (!NVectorStaly)
                            {
                                double nx = (double)NormalColors[P[0].x + P[0].y * wdth].r / 128.0 - 1.0;
                                double ny = (double)NormalColors[P[0].x + P[0].y * wdth].g / 128.0 - 1.0;
                                double nz = (double)NormalColors[P[0].x + P[0].y * wdth].b / 256.0;
                                p0N = (nx, ny, nz);

                                nx = (double)NormalColors[P[1].x + P[1].y * wdth].r / 128.0 - 1.0;
                                ny = (double)NormalColors[P[1].x + P[1].y * wdth].g / 128.0 - 1.0;
                                nz = (double)NormalColors[P[1].x + P[1].y * wdth].b / 256.0;
                                p1N = (nx, ny, nz);

                                nx = (double)NormalColors[P[2].x + P[2].y * wdth].r / 128.0 - 1.0;
                                ny = (double)NormalColors[P[2].x + P[2].y * wdth].g / 128.0 - 1.0;
                                nz = (double)NormalColors[P[2].x + P[2].y * wdth].b / 256.0;
                                p2N = (nx, ny, nz);
                            }

                            col = CalculateColorHybrid(j, scan, p0col, p1col, p2col, p0N, p1N, p2N,
                                p0dist, p1dist, p2dist, kd, ks, m, lightVec.x, lightVec.y, lightVec.z);

                        }

                        colorsList.Add((j + scan * wdth, (byte)col.r, (byte)col.g, (byte)col.b));

                    }
                }

            }

            unsafe
            {
                using (var context = Bitmap.GetBitmapContext())
                {
                    for (int i = 0; i < colorsList.Count; i++)
                    {
                        context.Pixels[colorsList[i].ind] = (255 << 24) | (colorsList[i].r << 16) | (colorsList[i].g << 8) | colorsList[i].b;
                    }
                }
            }
        }

        private (int r, int g, int b) CalculateColorHybrid(int j, int scan, (int r, int g, int b) p0col, 
            (int r, int g, int b) p1col, (int r, int g, int b) p2col, (double x, double y, double z) p0N, 
            (double x, double y, double z) p1N, (double x, double y, double z) p2N, double p0dist, double p1dist, 
            double p2dist, double kd, double ks, double m, double lightX = 0, double lightY = 0, double lightZ = 1)
        {
            var p0 = 1 / p0dist;
            var p1 = 1 / p1dist;
            var p2 = 1 / p2dist;

            int r = (int)((p0col.r * p0 + p1col.r * p1 + p2col.r * p2) / (p0 + p1 + p2));
            int g = (int)((p0col.g * p0 + p1col.g * p1 + p2col.g * p2) / (p0 + p1 + p2));
            int b = (int)((p0col.b * p0 + p1col.b * p1 + p2col.b * p2) / (p0 + p1 + p2));

            double Nx = (p0 * p0N.x + p1 * p1N.x + p2 * p2N.x) / (p0 + p1 + p2);
            double Ny = (p0 * p0N.y + p1 * p1N.y + p2 * p2N.y) / (p0 + p1 + p2);
            double Nz = (p0 * p0N.z + p1 * p1N.z + p2 * p2N.z) / (p0 + p1 + p2);

            (double x, double y, double z) vN = (Nx, Ny, Nz);
            vN = Normalize(vN);

            (double r, double g, double b) IL = (LightR / 255.0, LightG / 255.0, LightB / 255.0);
            (double x, double y, double z) vL = (lightX, lightY, lightZ);
            (double x, double y, double z) vV = (0, 0, 1);
            (double x, double y, double z) vR = (2 * Cos(vN, vL) * vN.x - vL.x, 2 * Cos(vN, vL) * vN.y - vL.y, 2 * Cos(vN, vL) * vN.z - vL.z);
            double cosNL = 1;
            double cosVR = 1;

            vR = (2 * Cos(vN, vL) * vN.x - vL.x, 2 * Cos(vN, vL) * vN.y - vL.y, 2 * Cos(vN, vL) * vN.z - vL.z);
            cosNL = Cos(vN, vL);
            cosVR = Cos(vV, vR);

            cosVR = Math.Pow(cosVR, m);

            int R = 0;
            int G = 0;
            int B = 0;

            if (!IsSpotlightOn)
            {
                R = (int)(255.0 * (kd * IL.r * (double)r * cosNL + ks * IL.r * (double)r * cosVR));
                G = (int)(255.0 * (kd * IL.g * (double)g * cosNL + ks * IL.g * (double)g * cosVR));
                B = (int)(255.0 * (kd * IL.b * (double)b * cosNL + ks * IL.b * (double)b * cosVR));
            }
            else
            {
                (double x, double y, double z) Lr = (-SpotR.x + j, -SpotR.y + scan, -SpotlightHeight);
                (double x, double y, double z) Lg = (-SpotG.x + j, -SpotG.y + scan, -SpotlightHeight);
                (double x, double y, double z) Lb = (-SpotB.x + j, -SpotB.y + scan, -SpotlightHeight);

                (double x, double y, double z) Vr = (Bitmap.PixelWidth / 2 - SpotR.x, Bitmap.PixelHeight / 2 - SpotR.y,
                    -SpotR.z);
                (double x, double y, double z) Vg = (Bitmap.PixelWidth / 2 - SpotG.x, Bitmap.PixelHeight / 2 - SpotG.y,
                    -SpotG.z);
                (double x, double y, double z) Vb = (Bitmap.PixelWidth / 2 - SpotB.x, Bitmap.PixelHeight / 2 - SpotB.y,
                    -SpotB.z);

                Lr = Normalize(Lr);
                Lg = Normalize(Lg);
                Lb = Normalize(Lb);
                Vr = Normalize(Vr);
                Vg = Normalize(Vg);
                Vb = Normalize(Vb);

                double ILr = Math.Pow(Cos(Lr, Vr), SpotlightFocus);
                double ILg = Math.Pow(Cos(Lg, Vg), SpotlightFocus);
                double ILb = Math.Pow(Cos(Lb, Vb), SpotlightFocus);


                R = (int)(255.0 * (kd * ILr * (double)r * cosNL + ks * ILr * (double)r * cosVR));
                G = (int)(255.0 * (kd * ILg * (double)g * cosNL + ks * ILg * (double)g * cosVR));
                B = (int)(255.0 * (kd * ILb * (double)b * cosNL + ks * ILb * (double)b * cosVR));
            }

            R = R > 255 ? 255 : R;
            G = G > 255 ? 255 : G;
            B = B > 255 ? 255 : B;

            R = R < 0 ? 0 : R;
            G = G < 0 ? 0 : G;
            B = B < 0 ? 0 : B;

            return (R, G, B);

        }

        private (int r, int g, int b) CalculateColorInterpolated(int j, int scan, (int r, int g, int b) p0col, 
            (int r, int g, int b) p1col, (int r, int g, int b) p2col, double kd, double ks, double m, 
            double p0dist, double p1dist, double p2dist)
        {
            var p0 = 1 / p0dist;
            var p1 = 1 / p1dist;
            var p2 = 1 / p2dist;

            int R = (int)((p0col.r * p0 + p1col.r * p1 + p2col.r * p2) / (p0 + p1 + p2));
            int G = (int)((p0col.g * p0 + p1col.g * p1 + p2col.g * p2) / (p0 + p1 + p2));
            int B = (int)((p0col.b * p0 + p1col.b * p1 + p2col.b * p2) / (p0 + p1 + p2));

            R = R > 255 ? 255 : R;
            G = G > 255 ? 255 : G;
            B = B > 255 ? 255 : B;

            R = R < 0 ? 0 : R;
            G = G < 0 ? 0 : G;
            B = B < 0 ? 0 : B;

            return (R, G, B);
        }

        private double Distance(int j, int scan, int x, int y)
        {
            return Math.Sqrt((j - x) * (j - x) + (scan - y) * (scan - y));
        }

        private (int r, int g, int b) CalculateColorExact(int pX, int pY,(int x, int y) p1, 
            (int x, int y) p2, (int x, int y) p3, double kd, double ks, double m, double lightX = 0, double lightY = 0, double lightZ = 1)
        {
            var wdth = 0;
            if (NormalBitmap != null)
                wdth = NormalBitmap.PixelWidth;
            var picWdth = 0;
            if(PictureBitmap != null)
                picWdth = PictureBitmap.PixelWidth;

            (double r, double g, double b) IL = (LightR / 255.0, LightG / 255.0, LightB / 255.0);
            (double r, double g, double b) IO = (DefR / 255.0, DefG / 255.0, DefB/255.0);
            (double x, double y, double z) vN = (0.0, 0.0, 1.0);
            (double x, double y, double z) vL = (lightX, lightY, lightZ);
            vL = Normalize(vL);
            (double x, double y, double z) vV = (0.0, 0.0, 1.0);
            (double x, double y, double z) vR = (0.0, 0.0, 1.0);



            if (!CzyStalyKolor)
            {
                double ior = (double) ((int) PictureColors[pX + pY * picWdth].r) / 255.0;
                double iog = (double) ((int) PictureColors[pX + pY * picWdth].g) / 255.0;
                double iob = (double) ((int) PictureColors[pX + pY * picWdth].b) / 255.0;

                IO = (ior, iog, iob);
            }

            if (!NVectorStaly)
            {
                double nx = (double) ((int) (NormalColors[pX + pY * picWdth].r)) / 128.0 - 1.0;
                double ny = (double) ((int) (NormalColors[pX + pY * picWdth].g)) / 128.0 - 1.0;
                double nz = (double) ((int) (NormalColors[pX + pY * picWdth].b)) / 256.0;

                vN = (nx, ny, nz);
                vN = Normalize(vN);
            }

            double cosNL = Cos(vN, vL);
            double rx = 2.0 * cosNL * vN.x - vL.x;
            double ry = 2.0 * cosNL * vN.y - vL.y;
            double rz = 2.0 * cosNL * vN.z - vL.z;

            vR = (rx, ry, rz);
            vR = Normalize(vR);

            double cosVR = Cos(vV, vR);
            cosVR = Math.Pow(cosVR, m);

            int R = 0;
            int G = 0;
            int B = 0;

            if (!IsSpotlightOn)
            {
                R = (int)(255.0 * (kd * IL.r * IO.r * cosNL + ks * IL.r * IO.r * cosVR));
                G = (int)(255.0 * (kd * IL.g * IO.g * cosNL + ks * IL.g * IO.g * cosVR));
                B = (int)(255.0 * (kd * IL.b * IO.b * cosNL + ks * IL.b * IO.b * cosVR));
            }
            else
            {
                (double x, double y, double z) Lr = (-SpotR.x + pX, -SpotR.y + pY, -SpotlightHeight);
                (double x, double y, double z) Lg = (-SpotG.x + pX, -SpotG.y + pY, -SpotlightHeight);
                (double x, double y, double z) Lb = (-SpotB.x + pX, -SpotB.y + pY, -SpotlightHeight);

                (double x, double y, double z) Vr = (Bitmap.PixelWidth / 2 - SpotR.x, Bitmap.PixelHeight / 2 - SpotR.y,
                    -SpotR.z);
                (double x, double y, double z) Vg = (Bitmap.PixelWidth / 2 - SpotG.x, Bitmap.PixelHeight / 2 - SpotG.y,
                    -SpotG.z);
                (double x, double y, double z) Vb = (Bitmap.PixelWidth / 2 - SpotB.x, Bitmap.PixelHeight / 2 - SpotB.y,
                    -SpotB.z);

                Lr = Normalize(Lr);
                Lg = Normalize(Lg);
                Lb = Normalize(Lb);
                Vr = Normalize(Vr);
                Vg = Normalize(Vg);
                Vb = Normalize(Vb);

                double ILr = Math.Pow(Cos(Lr, Vr), SpotlightFocus);
                double ILg = Math.Pow(Cos(Lg, Vg), SpotlightFocus);
                double ILb = Math.Pow(Cos(Lb, Vb), SpotlightFocus);


                R = (int)(255.0 * (kd * ILr * IO.r * cosNL + ks * ILr * IO.r * cosVR));
                G = (int)(255.0 * (kd * ILg * IO.g * cosNL + ks * ILg * IO.g * cosVR));
                B = (int)(255.0 * (kd * ILb * IO.b * cosNL + ks * ILb * IO.b * cosVR));
            }


            R = R > 255 ? 255 : R;
            G = G > 255 ? 255 : G;
            B = B > 255 ? 255 : B;

            R = R < 0 ? 0 : R;
            G = G < 0 ? 0 : G;
            B = B < 0 ? 0 : B;

            return (R, G, B);
        }

        private int GetX((int x, int y) v1, (int x, int y) v2, int y)
        {
            return ((y - v1.y) * (v2.x - v1.x) / (v2.y - v1.y) + v1.x);
        }

        private (double x, double y, double z) Normalize((double x, double y, double z) v)
        {
            double len = Math.Sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
            return (v.x / len, v.y / len, v.z / len);
        }


        private (int x, int y) CirclePoint(int radius, double angleInDegrees, (int x, int y) origin)
        {
            int x = (int) (radius * Math.Cos((double) angleInDegrees * Math.PI / 180.0)) + origin.x;
            int y = (int) (radius * Math.Sin((double) angleInDegrees * Math.PI / 180.0)) + origin.y;

            return (x, y);
        }

    }
}
