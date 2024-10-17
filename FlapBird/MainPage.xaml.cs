namespace FlapBird;

public partial class MainPage : ContentPage
{
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	const int gravidade = 1;

	const int maxPulo = 3;

	const int minOpen = 50;

	const int JumpStrengt = 20;

	const int fps = 20;


//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

	int speed = 6;
	int tempoPulando = 0;

	int score = 0;

	bool isJumping = false;	

	bool dead = true;
	
	double LarguraJanela=0;

	double AlturaJanela=0;



	public MainPage()
	{
		InitializeComponent();
	}

//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

	void Inicializar()
	{
		dead = false;
		Mario.TranslationY = 0;
		Drawn();
	}

		void OnGameOverClicked(object s, TappedEventArgs a)

		{
			FrameGameOver.IsVisible=false;
			Inicializar();
			Drawn();
			score = 0;
		}

	async Task Drawn()
	{
		while (!dead)
		{
			if(isJumping)
			{
				JumpApply();
			}
			else
			{
				CreateGravity();
				AndarPipe();
			}

			CreateGravity();
			AndarPipe();
			if(VerifyColisao())
			{
				dead = true;
				FrameGameOver.IsVisible = true;
				LabelFinalScore.IsVisible = true;
				break;
			}
			await Task.Delay(fps);

		}
	}

	async void CreateGravity()
	{
		Mario.TranslationY += gravidade;
	}
//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	void AndarPipe()
	{
		CanoCima.TranslationX -= speed;
		CanoBaixo.TranslationX -= speed;

		if (CanoCima.TranslationX < -LarguraJanela)
		{
			CanoCima.TranslationX = 0;
			CanoBaixo.TranslationX = 0;
			var MaxHeight = -100;
			var MinHeight = -CanoBaixo.HeightRequest;
			

			CanoCima.TranslationY = Random.Shared.Next((int)MaxHeight, (int)MinHeight);
			CanoBaixo.TranslationY = CanoCima.TranslationY + minOpen + CanoBaixo.HeightRequest;



			score++;
			LabelScore.Text = "Canos: " + score.ToString("D3");
			LabelScore.TextColor = Color.FromHex("#000000");

			LabelFinalScore.Text = "Final Score:" + score.ToString("D3");
			LabelFinalScore.TextColor = Color.FromHex("#000000");

		}

	}


		protected override void OnSizeAllocated(double AJ, double LJ)
		{
			base.OnSizeAllocated(AJ, LJ);
			LarguraJanela = LJ;
			AlturaJanela = AJ;	//isso que faz os canos andarem
		}
//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------



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

		bool VerifyColisao()
		{
			if (!dead)
			{
				if (VerificaColisaoTeto() || VerificaColisaoChao())
				{
					return true;
				}
			}
			return false;
		}
//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		void JumpApply()
		{
			Mario.TranslationY -= JumpStrengt;
			tempoPulando ++;
			if (tempoPulando > maxPulo)
			{
				isJumping = false;
				tempoPulando = 0;
			}
		}
		void Jump(object sender, TappedEventArgs a)
		{
			isJumping = true;
		}

}

