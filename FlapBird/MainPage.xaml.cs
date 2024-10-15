namespace FlapBird;

public partial class MainPage : ContentPage
{

	const int gravidade = 10;

	const int maxPulo = 3;

	int tempoPulando = 0;

	bool isJumping = false;	

	const int fps = 300;

	bool dead = true;
	
	double LarguraJanela;

	double AlturaJanela;

	int speed = 80;



	public MainPage()
	{
		InitializeComponent();
	}



	void Inicializar()
	{
		dead = false;
		Mario.TranslationY = 0;
		Drawn();
	}

	async Task Drawn()
	{
		while (!dead)
		{
			CreateGravity();
			AndarPipe();
			await Task.Delay(fps);

		}
	}

	async void CreateGravity()
	{
		Mario.TranslationY += gravidade;
	}

	void AndarPipe()
	{
		CanoCima.TranslationX -= speed;
		CanoBaixo.TranslationX -= speed;

		if (CanoCima.TranslationX < -LarguraJanela)
		{
			CanoCima.TranslationX = 0;
			CanoBaixo.TranslationX = 0;
		}

	}

		void OnGameOverClicked(object s, TappedEventArgs a)

		{
			FrameGameOver.IsVisible=false;
			Inicializar();
			Drawn();
		}

		protected override void OnSizeAllocated(double AJ, double LJ)
		{
			base.OnSizeAllocated(AJ, LJ);
			LarguraJanela = LJ;
			AlturaJanela = AJ;	//isso que faz os canos andarem
		}

		bool VerificaColisaoTeto()
		{
			var minY = -AlturaJanela/2;
			if (Mario.TranslationY <=minY)
			    return true;
			else
				return false;
		}
		bool VerificaColisaoChao()
		{
			var maxY = AlturaJanela/2;
			if (Mario.TranslationY >=maxY)
			return true;
			else
			return false;
		}

}

