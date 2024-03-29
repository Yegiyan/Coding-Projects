﻿using static Raylib_cs.Raylib;
using Raylib_cs;

class Program
{
    const int SCREEN_WIDTH = 800;
    const int SCREEN_HEIGHT = 600;

    enum State { ACTIVE, INACTIVE }
    static State state = State.INACTIVE;

    static double simulationSpeed = 10.0;
    static double accumulator = 0.0;

    static int gridWidth = 160;
    static int gridHeight = 120;

    static int cellWidth;
    static int cellHeight;

    static Cell[,] grid = new Cell[gridWidth, gridHeight];

    static void Main(string[] args)
    {
        InitWindow(SCREEN_WIDTH, SCREEN_HEIGHT, "Cellular Automata");

        cellWidth = SCREEN_WIDTH / gridWidth;
        cellHeight = SCREEN_HEIGHT / gridHeight;

        for (int x = 0; x < gridWidth; x++)
            for (int y = 0; y < gridHeight; y++)
                InitGrid(grid, x, y);

        while (!WindowShouldClose())
        {
            double timeStep = 1.0 / simulationSpeed;
            double deltaTime = GetFrameTime();
            accumulator += deltaTime;

            Input(grid);

            while (accumulator >= timeStep)
            {
                if (state == State.ACTIVE)
                    GameOfLife();

                accumulator -= timeStep;
            }

            BeginDrawing();
            ClearBackground(Color.Black);

            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    if (grid[x, y].IsAlive)
                        DrawRectangle(grid[x, y].X, grid[x, y].Y, grid[x, y].Width, grid[x, y].Height, Color.White);
                    else
                        DrawRectangle(grid[x, y].X, grid[x, y].Y, grid[x, y].Width, grid[x, y].Height, Color.Black);
                }
            }

            DrawText("Simulation:", 15, 15, 20, Color.LightGray);
            if (state == State.INACTIVE) DrawText("INACTIVE", 125, 15, 20, Color.Red);
            if (state == State.ACTIVE) DrawText("ACTIVE", 125, 15, 20, Color.Green);

            DrawText("Speed: " + simulationSpeed, 15, 40, 20, Color.LightGray);
            DrawText("Space to Start/Stop", 15, 65, 20, Color.LightGray);
            DrawText("C to Clear", 15, 90, 20, Color.LightGray);

            EndDrawing();
        }
        CloseWindow();
    }

    static void GameOfLife()
    {
        Cell[,] newGrid = new Cell[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                InitGrid(newGrid, x, y);

                int liveNeighbors = 0;

                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        if (dx == 0 && dy == 0)
                            continue;

                        int neighborX = x + dx;
                        int neighborY = y + dy;

                        if (neighborX >= 0 && neighborX < gridWidth && neighborY >= 0 && neighborY < gridHeight)
                            if (grid[neighborX, neighborY].IsAlive)
                                liveNeighbors++;
                    }
                }

                if (liveNeighbors < 2 && grid[x, y].IsAlive)
                    newGrid[x, y].IsAlive = false;

                else if (liveNeighbors >= 2 && liveNeighbors <= 3 && grid[x, y].IsAlive)
                    newGrid[x, y].IsAlive = true;

                else if (liveNeighbors > 3 && grid[x, y].IsAlive)
                    newGrid[x, y].IsAlive = false;

                else if (liveNeighbors == 3 && !grid[x, y].IsAlive)
                    newGrid[x, y].IsAlive = true;
            }
        }
        grid = newGrid;
    }

    static void InitGrid(Cell[,] grid, int x, int y)
    {
        grid[x, y] = new Cell(x, y, cellWidth, cellHeight, 0, false);
        grid[x, y].X = x * cellWidth;
        grid[x, y].Y = y * cellHeight;
        grid[x, y].Width = cellWidth;
        grid[x, y].Height = cellHeight;
    }

    static void Input(Cell[,] grid)
    {
        if (IsMouseButtonDown(MouseButton.Left))
        {
            int cellX = GetMouseX() / cellWidth;
            int cellY = GetMouseY() / cellHeight;

            if (cellX >= 0 && cellX < gridWidth && cellY >= 0 && cellY < gridHeight)
            {
                Cell cell = grid[cellX, cellY];
                cell.IsAlive = true;
            }
        }

        if (IsMouseButtonDown(MouseButton.Right))
        {
            int cellX = GetMouseX() / cellWidth;
            int cellY = GetMouseY() / cellHeight;

            if (cellX >= 0 && cellX < gridWidth && cellY >= 0 && cellY < gridHeight)
            {
                Cell cell = grid[cellX, cellY];
                cell.IsAlive = false;
            }
        }

        if (IsKeyPressed(KeyboardKey.Space) && state == State.INACTIVE)
            state = State.ACTIVE;

        else if (IsKeyPressed(KeyboardKey.Space) && state == State.ACTIVE)
            state = State.INACTIVE;

        if (IsKeyPressed(KeyboardKey.Right))
            simulationSpeed += 5.0;

        if (IsKeyPressed(KeyboardKey.Left) && simulationSpeed > 0)
            simulationSpeed -= 5.0;

        if (IsKeyPressed(KeyboardKey.C))
        {
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    grid[x, y].IsAlive = false;
                }
            }
        }
    }
}