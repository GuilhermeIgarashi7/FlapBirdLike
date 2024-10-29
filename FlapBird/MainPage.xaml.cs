﻿namespace FlapBird;

public partial class MainPage : ContentPage
{
    // Constantes
    const int gravidade = 20;
    const int maxPulo = 3;
    const int minOpen = 300;
    const int JumpStrengt = 40;
    const int fps = 50;

    // Variáveis
    int speed = 10;
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
            CanoBaixo.TranslationX = 0;
            CanoCima.TranslationX = 0;

            var MaxHeight = -(CanoBaixo.HeightRequest * 0.1);
            var MinHeight = -(CanoBaixo.HeightRequest * 0.8);

            CanoCima.TranslationY = Random.Shared.Next((int)MinHeight, (int)MaxHeight);
            CanoBaixo.TranslationY = CanoCima.TranslationY + minOpen + CanoBaixo.HeightRequest;

            score++;
            LabelScore.Text = "Score: " + score.ToString("D5");
            LabelFinalScore.Text = "Final Score: " + score.ToString("D5");

            if (score % 4 == 0)
                speed++;
        }
    }

    bool VerifyColisao()
    {
        return (!dead && (VerificaColisaoChao() || VerificaColisaoTeto() || VerificaCanoCima() || VerificaCanoBaixo()));
    }

    bool VerificaColisaoTeto()
    {
        var minY = -AlturaJanela / 2;
        return CachorroImage.TranslationY <= minY;
    }

    bool VerificaColisaoChao()
    {
        var maxY = AlturaJanela / 2;
        return CachorroImage.TranslationY >= maxY - CachorroImage.Height;
    }

    bool VerificaCanoCima()
    {
        var HorizontalCord = (LarguraJanela / 2) - (CachorroImage.WidthRequest / 2);
        var VerticalCord = (AlturaJanela / 2) - (CachorroImage.HeightRequest / 2) + CachorroImage.TranslationY;

        return (HorizontalCord >= Math.Abs(CanoCima.TranslationX) - CanoCima.WidthRequest &&
                HorizontalCord <= Math.Abs(CanoCima.TranslationX) + CanoCima.WidthRequest &&
                VerticalCord <= CanoCima.HeightRequest + CanoCima.TranslationY);
    }

    bool VerificaCanoBaixo()
    {
        var HorizontalCord = (LarguraJanela / 2) - (CachorroImage.WidthRequest / 2);
        var VerticalCord = (AlturaJanela / 2) + (CachorroImage.HeightRequest / 2) + CachorroImage.TranslationY;

        return (HorizontalCord >= Math.Abs(CanoCima.TranslationX) - CanoCima.WidthRequest &&
                HorizontalCord <= Math.Abs(CanoCima.TranslationX) + CanoCima.WidthRequest &&
                VerticalCord >= CanoCima.TranslationY + minOpen);
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
        _ = Drawn();
    }
}