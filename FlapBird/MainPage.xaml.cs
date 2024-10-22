namespace FlapBird;

public partial class MainPage : ContentPage
{
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	const int gravidade = 1;

	const int maxPulo = 3;

	const int minOpen = 50;

	const int JumpStrengt = 20;

	const int fps = 24;


//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

	int speed = 4;
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

		LabelFinalScore.IsVisible = false;
		dead = false;
		FenixImage.TranslationY = 0;
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
			CreateGravity();
			AndarPipe();
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
		FenixImage.TranslationY += gravidade;
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


			LabelFinalScore.Text = "Final Score:" + score.ToString("D3");


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
			if (FenixImage.TranslationY <=minY)
			    return true;
			else
				return false;
		}
		bool VerificaColisaoChao()
		{
			var maxY = AlturaJanela/2;
			if (FenixImage.TranslationY >=maxY)
			return true;
			else
			return false;
		}

		bool VerifyColisao()
		{
			if (!dead)
			{
				if (VerificaColisaoTeto() || VerificaColisaoChao()|| VerificaCanoCima())
				{
					return true;
				}
			}
			return false;
		}


		bool VerificaCanoCima()
		{
			var HorizontalCord = (LarguraJanela/2)-(FenixImage.WidthRequest/2);
			var VerticalCord = (AlturaJanela/2)-(FenixImage.HeightRequest/2)+FenixImage.TranslationY;

			if (HorizontalCord >= Math.Abs(CanoCima.TranslationX)-CanoCima.WidthRequest &&
				HorizontalCord <= Math.Abs(CanoCima.TranslationX)+CanoCima.WidthRequest &&
				VerticalCord   <= CanoCima.HeightRequest + CanoCima.TranslationY)
				{
					return true;
				}
				else
				{
					return false;
				}


		}
//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		void JumpApply()
		{
			FenixImage.TranslationY -= JumpStrengt;
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

