﻿namespace FlapBird;

public partial class MainPage : ContentPage
{
    // Constantes
    const int gravidade = 5;
    const int maxPulo = 3;
    const int minOpen = 200;
    const int JumpStrengt = 30;
    const int fps = 25;

    // Variáveis
    int speed = 5;
    int tempoPulando = 0;
    int score = 0;
    bool isJumping = false;
    bool dead = true;

    double LarguraJanela = 0;
    double AlturaJanela = 0;

    public MainPage()
    {
        InitializeComponent();
    }

    protected override void OnSizeAllocated(double AJ, double LJ)
    {
        base.OnSizeAllocated(AJ, LJ);
        LarguraJanela = LJ;
        AlturaJanela = AJ;

        if (AJ > 0)
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
        CanoCima.TranslationX = -LarguraJanela;
        CachorroImage.TranslationX = 0;
        CachorroImage.TranslationY = 0;
        score = 0;

        AndarPipe();
    }

    async Task Drawn()
    {
        while (!dead)
        {
            if (isJumping)
                JumpApply();
            else
                CreateGravity();

            AndarPipe();

            if (VerifyColisao())
            {
                dead = true;
                FrameGameOver.IsVisible = true;
                LabelFinalScore.IsVisible = true;
                break;
            }

            await Task.Delay(fps);
        }
    }

    void CreateGravity()
    {
        CachorroImage.TranslationY += gravidade;
    }

    void JumpApply()
    {
        CachorroImage.TranslationY -= JumpStrengt;
        tempoPulando++;
        if (tempoPulando > maxPulo)
        {
            isJumping = false;
            tempoPulando = 0;
        }
    }

    void AndarPipe()
    {
        CanoCima.TranslationX -= speed;
        CanoBaixo.TranslationX -= speed;

        if (CanoBaixo.TranslationX < -LarguraJanela)
        {
            CanoCima.TranslationX = 20;
            CanoBaixo.TranslationX = 20;

            var MaxHeight = -(CanoBaixo.HeightRequest * 0.1);
            var MinHeight = -(CanoBaixo.HeightRequest * 0.8);

            CanoCima.TranslationY = Random.Shared.Next((int)MinHeight, (int)MaxHeight);
            CanoBaixo.TranslationY = CanoCima.TranslationY + CanoBaixo.HeightRequest + minOpen;

            score++;
            LabelScore.Text = "Score: " + score.ToString("D5");
            LabelFinalScore.Text = "Final Score: " + score.ToString("D5");

            if (score % 4 == 0)
                speed++;
        }
    }

    bool VerifyColisao()
    {
        return (!dead && (VerificaColisaoChao() || VerificaColisaoTeto() || VerificaColisaoCano()));
    }

	bool VerificaColisaoCano()
	{
		if(VerificaCanoCima() || VerificaCanoBaixo())
			return true;
		else 
			return false;
	}


    bool VerificaColisaoTeto()
    {
        var minY = -AlturaJanela / 2;
        if (CachorroImage.TranslationY <= minY)
        return true;
        else
        return false;
    }

    bool VerificaColisaoChao()
    {
        var maxY = AlturaJanela / 2;
        if(CachorroImage.TranslationY >= maxY)
        return true;
        else
        return false;


    }

    bool VerificaCanoCima()
    {
        

		var HorizontalCord= LarguraJanela - 50 - (CachorroImage.WidthRequest / 2);
		var VerticalCord = (AlturaJanela / 2) - (CachorroImage.HeightRequest / 2) + CachorroImage.TranslationY;

		if (HorizontalCord >= Math.Abs(CanoCima.TranslationX) - CanoCima.WidthRequest &&
		HorizontalCord <= Math.Abs(CanoCima.TranslationX) + CanoCima.WidthRequest &&
		VerticalCord <= CanoCima.HeightRequest + CanoCima.TranslationY)
        	return true;
		else
			return false;

    }

    bool VerificaCanoBaixo()
    {


		var PosicaoH = LarguraJanela - 50 - CachorroImage.WidthRequest / 2;
		var PosicaoV = (AlturaJanela / 2) + (CachorroImage.HeightRequest / 2) + CachorroImage.TranslationY;
		var yMaxCano = CanoCima.HeightRequest + CanoCima.TranslationY + minOpen;
		if (PosicaoH >= Math.Abs(CanoCima.TranslationX) - CanoCima.WidthRequest &&
		 	PosicaoH <= Math.Abs(CanoCima.TranslationX) + CanoCima.WidthRequest &&
		 	PosicaoH >= yMaxCano)
		{
			return true;
		}
		else
		{
			return false;
		}
    }

    void Jump(object sender, TappedEventArgs args)
    {
        isJumping = true;
    }

    void OnGameOverClicked(object s, TappedEventArgs args)
    {
        FrameGameOver.IsVisible = false;
        dead = false;
        Inicializar();
        Drawn();
    }
}