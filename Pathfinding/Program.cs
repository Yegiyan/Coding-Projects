using static Raylib_cs.Raylib;
using Raylib_cs;

class Program
{
    const int SCREEN_WIDTH = 800;
    const int SCREEN_HEIGHT = 600;

    static double simulationSpeed = 10.0;
    static double accumulator = 0.0;

    static void Main(string[] args)
    {
        InitWindow(SCREEN_WIDTH, SCREEN_HEIGHT, "Pathfinding");

        while (!WindowShouldClose())
        {
            double timeStep = 10.0 / simulationSpeed;
            double deltaTime = GetFrameTime();
            accumulator += deltaTime;

            while (accumulator >= timeStep)
            {
                // update cells here
                accumulator -= timeStep;
            }

            BeginDrawing();
            ClearBackground(Color.RayWhite);
            // draw cells here
            EndDrawing();
        }
        CloseWindow();
    }
}