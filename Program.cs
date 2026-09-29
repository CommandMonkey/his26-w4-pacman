using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace pacman
{
    class Program
    {
        // Screen properties
        private const float defScreenW = 828;
        private const float defScreenH = 900;
        public static uint screenW = (uint)defScreenW;
        public static uint screenH = (uint)defScreenH;
        private static Vector2f viewPos = new Vector2f((defScreenW/4)+18, defScreenH/4); //+18 since tutorial wants offset by one tile
        public static Vector2f viewSize = new Vector2f(defScreenW/2, defScreenH/2);
        
        // Game stuff
        private static Scene scene = new Scene();
        
        // Misc.
        private static Clock dtClock;
        
        static void Main(string[] args)
        {
            using (
                var window = new RenderWindow( new VideoMode(screenW, screenH), "Pacman" )
            )
            {
                // Setup event handlers
                window.Closed += (o, e) => window.Close(); // o: The object that triggered the event (sender);  e: the event data containing ex. what happened
                
                window.Resized += (o, e) =>
                {
                    screenW = e.Width;
                    screenH = e.Height;

                    UpdateView(window);
                };
                
                // Set the view zoom
                UpdateView(window);
                
                // Instantiate setup objects
                dtClock = new Clock();
                
                // Game stuff
                scene.Loader.Load("maze.txt");
                
                // Main Loop
                while (window.IsOpen)
                {
                    // Calculate Δt
                    float deltaTime = dtClock.Restart().AsSeconds();
                    
                    // React to events
                    window.DispatchEvents();
                    
                    // Update
                    Update(deltaTime);
                    
                    // Clear & Fill
                    window.Clear(new Color(29, 13, 74)); // #1d0d4a
                    
                    // Draw to window
                    Draw(window);
                    
                    // "Render the window to the screen"
                    window.Display();
                }
            }
        }
        
        private static void UpdateView(RenderTarget window)
        {
            float scale = MathF.Min(
                window.Size.X / defScreenW,
                window.Size.Y / defScreenH
            );

            float viewportW = (defScreenW * scale) / window.Size.X;
            float viewportH = (defScreenH * scale) / window.Size.Y;

            var view = new View(viewPos, viewSize);
            view.Viewport = new FloatRect(
                (1f - viewportW) * 0.5f,
                (1f - viewportH) * 0.5f,
                viewportW,
                viewportH
            );
            window.SetView(view);
        }

        static void Update(float deltaTime)
        {
            scene.UpdateAll(deltaTime);
        }

        static void Draw(RenderWindow target)
        {
            scene.RenderAll(target);
        }
    }
}
