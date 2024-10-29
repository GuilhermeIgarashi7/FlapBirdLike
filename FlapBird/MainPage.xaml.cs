namespace FlapBird;

public partial class MainPage : ContentPage
{
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	const int gravidade = 20;

	const int maxPulo = 3;

	const int minOpen = 200;

	const int JumpStrengt = 40;

	const int fps = 50;


//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

	int speed = 10;
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
		protected override void OnSizeAllocated(double AJ, double LJ)
		{
			base.OnSizeAllocated(AJ, LJ);
			LarguraJanela = LJ;
			AlturaJanela = AJ;	//isso que faz os canos andarem

			if(AJ > 0)
			{
				CanoCima.HeightRequest = AJ - CachorroImage.HeightRequest;
				CanoBaixo.HeightRequest = AJ - CachorroImage.HeightRequest;

			}
		}

	void Inicializar()
	{
		FrameGameOver.IsVisible = false;
		LabelFinalScore.IsVisible = false;
		CanoBaixo.TranslationX = -LarguraJanela;
		CanoCima.TranslationY = -LarguraJanela;
		CachorroImage.TranslationX = 0;
		CachorroImage.TranslationY = 0;
		score = 0;
		
		AndarPipe();
	}

	async Task Drawn()
	{
		while (!dead)
		{
			if(isJumping)
			JumpApply();
			
			else
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
		CachorroImage.TranslationY += gravidade;
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

			var MaxHeight = -(CanoBaixo.HeightRequest * 0.1);
			var MinHeight = -(CanoBaixo.HeightRequest * 0.8);
			

			CanoCima.TranslationY = Random.Shared.Next((int)MaxHeight, (int)MinHeight);
			CanoBaixo.TranslationY = CanoCima.TranslationY + minOpen + CanoBaixo.HeightRequest;



			score++;
			LabelScore.Text = "Canos: " + score.ToString("D3");


			LabelFinalScore.Text = "Final Score:" + score.ToString("D3");


		}

	}


//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

		bool VerifyColisao()
		{

		return (!dead && (VerificaColisaoTeto() || VerificaColisaoChao()|| VerificaCanoCima()));

		}


		bool VerificaColisaoTeto()
		{
			var minY = -AlturaJanela/2;
			if (CachorroImage.TranslationY <=minY)
			    return true;
			else
				return false;
		}
		bool VerificaColisaoChao()
		{
			var maxY = AlturaJanela/2;
			if (CachorroImage.TranslationY >=maxY)
			return true;
			else
			return false;
		}


		bool VerificaCanoCima()
		{
			var HorizontalCord = (LarguraJanela/2)-(CachorroImage.WidthRequest/2);
			var VerticalCord = (AlturaJanela/2)-(CachorroImage.HeightRequest/2)+CachorroImage.TranslationY;

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
			CachorroImage.TranslationY -= JumpStrengt;
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

//--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		void OnGameOverClicked(object s, TappedEventArgs a)

		{	

			FrameGameOver.IsVisible=false;
			dead = false;
			Inicializar();
			Drawn();

		}

}

