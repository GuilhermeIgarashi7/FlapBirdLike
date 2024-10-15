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


}

