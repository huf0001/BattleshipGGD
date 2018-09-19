using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using SwinGameSDK;

public static class GameResources
{
    private static Dictionary<string, Bitmap> _Images = new Dictionary<string, Bitmap>();
    private static Dictionary<string, Font> _Fonts = new Dictionary<string, Font>();
    private static Dictionary<string, SoundEffect> _Sounds = new Dictionary<string, SoundEffect>();
    private static Dictionary<string, Music> _Music = new Dictionary<string, Music>();

    private static Bitmap _Background;
    private static Bitmap _Animation;
    private static Bitmap _LoaderFull;
    private static Bitmap _LoaderEmpty;
    private static Font _LoadingFont;
    private static SoundEffect _StartSound;

    /// <summary>
    ///     ''' The Resources Class stores all of the Games Media Resources, such as Images, Fonts
    ///     ''' Sounds, Music.
    ///     ''' </summary>
    public static void LoadResources()
    {
        int width = 800;
        int height = 600;

        // TODO: crashes when run on Windows; ScreenWidth() and ScreenHeight()
        // create an exceptions within SwinGame ("Unable to find an entry point
        // named 'sg_WindowManager_ScreenWidth' in DLL 'sgsdk.dll'..."
        width = System.Convert.ToInt32(SwinGame.ScreenWidth());
        height = System.Convert.ToInt32(SwinGame.ScreenHeight());

        SwinGame.ChangeScreenSize(800, 600);

        ShowLoadingScreen();

        ShowMessage("Loading fonts...", 0);
        LoadFonts();
        SwinGame.Delay(100);

        ShowMessage("Loading images...", 1);
        LoadImages();
        SwinGame.Delay(100);

        ShowMessage("Loading sounds...", 2);
        LoadSounds();
        SwinGame.Delay(100);

        ShowMessage("Loading music...", 3);
        LoadMusic();
        SwinGame.Delay(100);

        SwinGame.Delay(100);
        ShowMessage("Game loaded...", 5);
        SwinGame.Delay(100);
        EndLoadingScreen(width, height);
    }

    /// <summary>
    ///     ''' Loads the game's required fonts.
    ///     ''' </summary>
    private static void LoadFonts()
    {
        NewFont("ArialLarge", "arial.ttf", 80);
        NewFont("Courier", "cour.ttf", 14);
        NewFont("CourierSmall", "cour.ttf", 8);
        NewFont("Menu", "ffaccess.ttf", 8);
    }
	
	/// <summary>
    ///     ''' Loads the game's required images.
	///     ''' This includes backgrounds, buttons,
	///     ''' ships, and visual effects.
    ///     ''' </summary>
    private static void LoadImages()
    {
        // Backgrounds
        NewImage("Menu", "main_page.jpg");
        NewImage("Discovery", "discover.jpg");
        NewImage("Deploy", "deploy.jpg");

        // Deployment
        NewImage("LeftRightButton", "deploy_dir_button_horiz.png");
        NewImage("UpDownButton", "deploy_dir_button_vert.png");
        NewImage("SelectedShip", "deploy_button_hl.png");
        NewImage("PlayButton", "deploy_play_button.png");
        NewImage("RandomButton", "deploy_randomize_button.png");

        // Ships
        int i;
        for (i = 1; i <= 5; i++)
        {
            NewImage("ShipLR" + i, "ship_deploy_horiz_" + i + ".png");
            NewImage("ShipUD" + i, "ship_deploy_vert_" + i + ".png");
        }

        // Explosions
        NewImage("Explosion", "explosion.png");
        NewImage("Splash", "splash.png");
    }

	/// <summary>
    ///     ''' Loads the game's required sounds.
	///     ''' Most of these are used in the
	///     ''' gameplay state.
    ///     ''' </summary>
    private static void LoadSounds()
    {
        NewSound("Error", "error.wav");
        NewSound("Hit", "hit.wav");
        NewSound("Sink", "sink.wav");
        NewSound("Siren", "siren.wav");                 //added a file siren.wav to the resources folder            
        NewSound("Miss", "watershot.wav");
        NewSound("Winner", "winner.wav");
        NewSound("Lose", "lose.wav");
    }
	
	/// <summary>
    ///     ''' Loads the game's required background music.
    ///     ''' </summary>
    private static void LoadMusic()
    {
        NewMusic("Background", "horrordrone.ogg");      //swapped .mp3 with .ogg file
    }

    /// <summary>
    ///     ''' Loads a new font to use in the program.
    ///     ''' </summary>
    ///     ''' <param name="fontName">Name of new font to be loaded.</param>
    ///     ''' <param name="filename">Name of file containing new font.</param>
    ///     ''' <param name="size">Size to use for new font.</param>
    private static void NewFont(string fontName, string filename, int size)
    {
        _Fonts.Add(fontName, SwinGame.LoadFont(SwinGame.PathToResource(filename, ResourceKind.FontResource), size));
    }

    /// <summary>
    ///     ''' Loads a new image to use in the program.
    ///     ''' </summary>
    ///     ''' <param name="imageName">Name of new image to be loaded.</param>
    ///     ''' <param name="filename">Name of file containing new image.</param>
    private static void NewImage(string imageName, string filename)
    {
        _Images.Add(imageName, SwinGame.LoadBitmap(SwinGame.PathToResource(filename, ResourceKind.BitmapResource)));
    }

    /// <summary>
    ///     ''' Loads a new image to use in the program.
    ///     ''' Functions identically to NewImage currently.
    ///     ''' </summary>
    ///     ''' <param name="imageName">Name of new image to be loaded.</param>
    ///     ''' <param name="fileName">Name of file containing new image.</param>
    ///     ''' <param name="transColor">Holds a colour. Currently not used in function.</param>
    private static void NewTransparentColorImage(string imageName, string fileName, Color transColor)
    {
        _Images.Add(imageName, SwinGame.LoadBitmap(SwinGame.PathToResource(fileName, ResourceKind.BitmapResource)));
    }

    /// <summary>
    ///     ''' Calls NewTransparentColorImage.
    ///     ''' Presumably used to catch function calls
    ///     ''' regardless of English or American spelling.
    ///     ''' </summary>
    private static void NewTransparentColourImage(string imageName, string fileName, Color transColor)
    {
        NewTransparentColorImage(imageName, fileName, transColor);
    }

    /// <summary>
    ///     ''' Loads a new sound to use in the program.
    ///     ''' </summary>
    ///     ''' <param name="soundName">Name of new sound to be loaded.</param>
    ///     ''' <param name="filename">Name of file containing new sound.</param>
    private static void NewSound(string soundName, string filename)
    {
        _Sounds.Add(soundName, Audio.LoadSoundEffect(SwinGame.PathToResource(filename, ResourceKind.SoundResource)));
    }

    /// <summary>
    ///     ''' Loads a new music track to use in the program.
    ///     ''' </summary>
    ///     ''' <param name="musicName">Name of new track to be loaded.</param>
    ///     ''' <param name="filename">Name of file containing new track.</param>
    private static void NewMusic(string musicName, string filename)
    {
        _Music.Add(musicName, Audio.LoadMusic(SwinGame.PathToResource(filename, ResourceKind.SoundResource)));
    }

    /// <summary>
    ///     ''' Gets a font from those loaded in the resources.
    ///     ''' </summary>
    ///     ''' <param name="font">Name of Font</param>
    ///     ''' <returns>The Font Loaded with this Name</returns>
    public static Font GameFont(string font)
    {
        return _Fonts[font];
    }

    /// <summary>
    ///     ''' Gets an Image loaded in the Resources
    ///     ''' </summary>
    ///     ''' <param name="image">Name of image</param>
    ///     ''' <returns>The image loaded with this name</returns>
    public static Bitmap GameImage(string image)
    {
        return _Images[image];
    }

    /// <summary>
    ///     ''' Gets an sound loaded in the Resources
    ///     ''' </summary>
    ///     ''' <param name="sound">Name of sound</param>
    ///     ''' <returns>The sound with this name</returns>
    public static SoundEffect GameSound(string sound)
    {
        return _Sounds[sound];
    }

    /// <summary>
    ///     ''' Gets the music loaded in the Resources.
    ///     ''' </summary>
    ///     ''' <param name="music">Name of music</param>
    ///     ''' <returns>The music with this name</returns>
    public static Music GameMusic(string music)
    {
        return _Music[music];
    }

    /// <summary>
    ///     ''' Shows the game's loading screen.
    ///     ''' Used during state transitions.
    ///     ''' Includes fonts, backgrounds and sound effects.
    ///     ''' </summary>
    ///     ''' <value name="_Background">The background to display on the loading screen.</value>
    ///     ''' <value name="_Animation">The animation to display on the loading screen.</value>
    ///     ''' <value name="_LoadingFont">The font to use on the loading screen.</value>
    ///     ''' <value name="_StartSound">The sound effect to play during the loading screen.</value>
    ///     ''' <value name="_LoaderFull">The image to use when fully loaded.</value>
    ///     ''' <value name="_LoaderEmpty">The image to use when 0% loaded.</value>
    private static void ShowLoadingScreen()
    {
        _Background = SwinGame.LoadBitmap(SwinGame.PathToResource("SplashBack.png", ResourceKind.BitmapResource));
        SwinGame.DrawBitmap(_Background, 0, 0);
        SwinGame.RefreshScreen();
        SwinGame.ProcessEvents();

        _Animation = SwinGame.LoadBitmap(SwinGame.PathToResource("SwinGameAni.jpg", ResourceKind.BitmapResource));
        _LoadingFont = SwinGame.LoadFont(SwinGame.PathToResource("arial.ttf", ResourceKind.FontResource), 12);
        _StartSound = Audio.LoadSoundEffect(SwinGame.PathToResource("SwinGameStart.wav", ResourceKind.SoundResource));

        _LoaderFull = SwinGame.LoadBitmap(SwinGame.PathToResource("loader_full.png", ResourceKind.BitmapResource));
        _LoaderEmpty = SwinGame.LoadBitmap(SwinGame.PathToResource("loader_empty.png", ResourceKind.BitmapResource));

        PlaySwinGameIntro();
    }

    /// <summary>
    ///     ''' Plays the SwinGame intro sequence.
    ///     ''' </summary>
    ///     ''' <value name="_ANI_CELL_COUNT">The number of cells to play the animation for.</value>
    private static void PlaySwinGameIntro()
    {
        const int ANI_CELL_COUNT = 11;

        Audio.PlaySoundEffect(_StartSound);
        SwinGame.Delay(1200);

        int i;
        for (i = 0; i <= ANI_CELL_COUNT - 1; i++)
        {
            SwinGame.DrawBitmap(_Background, 0, 0);
            SwinGame.Delay(20);
            SwinGame.RefreshScreen();
            SwinGame.ProcessEvents();
        }

        SwinGame.Delay(1500);
    }

    /// <summary>
    ///     ''' Draws a string as a message
    ///     ''' and shows it on screen.
    ///     ''' </summary>
    ///     ''' <param name="message">Message to be shown</param>
    ///     ''' <param name="number">Unknown</param>
    ///     ''' <value name="TX">X location to draw message at.</value>
    ///     ''' <value name="TY">Y location to draw message at.</value>
    ///     ''' <value name="TW">Width to draw message with.</value>
    ///     ''' <value name="TH">Height to draw message with.</value>
    ///     ''' <value name="STEPS">Number of steps to split loading screen graphic into.</value>
    ///     ''' <value name="BG_X">X location to draw background at.</value>
    ///     ''' <value name="BG_Y">Y location to draw background at.</value>
    ///     ''' <value name="fullW">Full width of loading screen graphic.</value>
    ///     ''' <value name="toDraw">Measurements of message box to draw.</value>
    private static void ShowMessage(string message, int number)
    {
        const int TX = 310;
        const int TY = 493;
        const int TW = 200;
        const int TH = 25;
        const int STEPS = 5;
        const int BG_X = 279;
        const int BG_Y = 453;

        int fullW;
        Rectangle toDraw = default(Rectangle);

        fullW = 260 * number / STEPS;
        SwinGame.DrawBitmap(_LoaderEmpty, BG_X, BG_Y);
        SwinGame.DrawCell(_LoaderFull, 0, BG_X, BG_Y);
        // SwinGame.DrawBitmapPart(_LoaderFull, 0, 0, fullW, 66, BG_X, BG_Y)

        toDraw.X = TX;
        toDraw.Y = TY;
        toDraw.Width = TW;
        toDraw.Height = TH;
        SwinGame.DrawText(message, Color.White, Color.Transparent, _LoadingFont, FontAlignment.AlignCenter, toDraw);
        // SwinGame.DrawText(message, Color.White, Color.Transparent, _LoadingFont, FontAlignment.AlignCenter, TX, TY, TW, TH)

        SwinGame.RefreshScreen();
        SwinGame.ProcessEvents();
    }

	/// <summary>
    ///     ''' Dictates what happens when
	///     ''' a loading screen ends.
    ///     ''' </summary>
	///     ''' <param name="width">New screen width after loading screen.</param>
    ///     ''' <param name="height">New screen height after loading screen.</param>
    private static void EndLoadingScreen(int width, int height)
    {
        SwinGame.ProcessEvents();
        SwinGame.Delay(500);
        SwinGame.ClearScreen();
        SwinGame.RefreshScreen();
        SwinGame.FreeFont(_LoadingFont);
        SwinGame.FreeBitmap(_Background);
        SwinGame.FreeBitmap(_Animation);
        SwinGame.FreeBitmap(_LoaderEmpty);
        SwinGame.FreeBitmap(_LoaderFull);

        // Throws AccessViolation exception, trying to read or write from protected memory.
        // Going to comment this out for now.
        // Note: already tried SwinGame.FreeSoundEffect(). That doesn't work either.

        // Audio.FreeSoundEffect(_StartSound);

        SwinGame.ChangeScreenSize(width, height);
    }

    /// <summary>
    ///     ''' Calls the four previous functions
	///     ''' to free the loaded resources.
    ///     ''' </summary>
    public static void FreeResources()
    {
        FreeFonts();
        FreeImages();
        FreeMusic();
        FreeSounds();
        SwinGame.ProcessEvents();
    }

    /// <summary>
    ///     ''' Calls FreeFont to free the
    ///     ''' resources used by each loaded font.
    ///     ''' </summary>
    private static void FreeFonts()
    {
        foreach (Font f in _Fonts.Values)
            SwinGame.FreeFont(f);
    }

	/// <summary>
    ///     ''' Calls FreeBitmap to free the
	///     ''' resources used by each loaded image.
    ///     ''' </summary>
    private static void FreeImages()
    {
        foreach (Bitmap b in _Images.Values)
            SwinGame.FreeBitmap(b);
    }

	/// <summary>
    ///     ''' Calls FreeSoundEffect to free the
	///     ''' resources used by each loaded sound effect.
    ///     ''' </summary>
    private static void FreeSounds()
    {
        foreach (SoundEffect s in _Sounds.Values)
        {
// Throws AccessViolation exception, trying to read or write from protected memory.
// Going to comment this out for now.
// Note: already tried SwinGame.FreeSoundEffect(). That doesn't work either.

            // Audio.FreeSoundEffect(s);
        }
    }

	/// <summary>
    ///     ''' Calls FreeMusic to free the
	///     ''' resources used by each loaded music track.
    ///     ''' </summary>
    private static void FreeMusic()
    {
        foreach (Music m in _Music.Values)
            Audio.FreeMusic(m);
    }
}
