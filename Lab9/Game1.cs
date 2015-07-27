#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Lab9
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;


		Texture2D agentTex,DragonTex,healthMushroomTex,poisonMushroomTex,fireballTex,bulletTex,backgroundTex;
		SpriteFont life,timeLeft,counDown;

		Texture2D startScreen,gameOverScreen,youWonScreen;

		Rectangle agent,Dragon,bullet;
		Rectangle helthMushroom1,helthMushroom2;
		Rectangle poisonMush1,poisonMush2,poisonMus3;
		Rectangle fireball;

		Vector2 background;
		Vector2 timeRemain,countDownPosition;

		int fireTime,startTime,secondPassed,timeRemains;
		int agentLife = 5;
		int dragonLife = 15;

		int lifeBucket =0;

		int playerRunningSpeed= 2;

		string gameState = "Start";

		KeyboardState keyState;

		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";	            
			//graphics.IsFullScreen = true;		
			graphics.PreferredBackBufferHeight=580;
			graphics.PreferredBackBufferWidth = 1024;

		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize ()
		{
			// TODO: Add your initialization logic here
			base.Initialize ();
				
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent ()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch (GraphicsDevice);

			//TODO: use this.Content to load your game content here 

			agentTex = Content.Load<Texture2D> ("Textures\\windy");
			DragonTex = Content.Load<Texture2D> ("Textures\\dragon");
			healthMushroomTex = Content.Load<Texture2D> ("Textures\\healthy_mushroom");
			poisonMushroomTex = Content.Load<Texture2D> ("Textures\\poisonus_mushroom");
			fireballTex = Content.Load<Texture2D> ("Textures\\fireball");
			bulletTex = Content.Load<Texture2D> ("Textures\\bullet");
			backgroundTex = Content.Load<Texture2D> ("Screens\\background");
			startScreen = Content.Load<Texture2D> ("Screens\\start");
			gameOverScreen = Content.Load<Texture2D> ("Screens\\go");
			youWonScreen = Content.Load<Texture2D> ("Screens\\won");

			agent = new Rectangle (50, 320, 50, 75);
			Dragon = new Rectangle (850, 290, 80, 105);
			fireball = new Rectangle (850, 330, 30, 30);
			bullet = new Rectangle (50, 330, 30, 30);

			helthMushroom1= new Rectangle (500, 10, 40, 40);
			helthMushroom2= new Rectangle (700, 20, 40, 40);

			poisonMush1= new Rectangle (300, 30, 40, 40);
			poisonMush2= new Rectangle (400, 30, 40, 40);
			poisonMus3 = new Rectangle (600, 30, 40, 40);

			background = new Vector2 (0.0f,0.0f);
			timeRemain = new Vector2 (800.0f,50.0f);
			countDownPosition = new Vector2 (50.0f,50.0f);

			startTime = 180 * 1000;
			secondPassed = 0;
			timeRemains = 0;
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update (GameTime gameTime)
		{
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			// Exit() is obsolete on iOS
			#if !__IOS__
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
			    Keyboard.GetState ().IsKeyDown (Keys.Escape)) {
				Exit ();
			}
			#endif
			// TODO: Add your update logic here		

			keyState = Keyboard.GetState ();

			if (gameState == "Start" && keyState.IsKeyDown (Keys.Space)) {
				gameState = "Running";
			}

			if (keyState.IsKeyDown (Keys.Left)) {
				agent.X -= playerRunningSpeed;
				bullet.X = agent.X;
			}

			if (keyState.IsKeyDown (Keys.Right)) {
				agent.X += playerRunningSpeed;
				bullet.X = agent.X;
			}

			if (keyState.IsKeyDown (Keys.Up)) {
				agent.Y -= playerRunningSpeed;
				bullet.Y = agent.Y;
			}

			if (keyState.IsKeyDown (Keys.Down)) {
				agent.Y += playerRunningSpeed;
				bullet.Y = agent.Y;
			}

			fireball.X -= 2;

			if (fireTime % 500 == 0) {
				fireball = new Rectangle (850, 380, 30, 30);
			}

			helthMushroom1.Y += 2;
			helthMushroom2.Y += 3;
			poisonMush1.Y += 4;
			poisonMush2.Y += 3;
			poisonMus3.Y += 2;
			if (fireTime % 2200 == 0) {
				helthMushroom1 = new Rectangle (500, 10, 60, 60);
				helthMushroom2 = new Rectangle (700, 20, 60, 60);
				poisonMush1 = new Rectangle(300, 30, 60, 60);
				poisonMush2 = new Rectangle(400, 30, 60, 60);
				poisonMus3 = new Rectangle(600, 30, 60, 60);
			}

			if (fireTime % 3200 == 0)
			{
				helthMushroom1 = new Rectangle(500, 10, 60, 60);
				helthMushroom2 = new Rectangle(700, 20, 60, 60);
			}

			fireTime += gameTime.ElapsedGameTime.Milliseconds;
			secondPassed += gameTime.ElapsedGameTime.Milliseconds;
			timeRemains = startTime - fireTime;

			if (timeRemains < 0)
			{
				this.Exit();
			}

			if (keyState.IsKeyDown(Keys.S))
			{
				bullet.X += 10;
			}

			if (fireball.Intersects(bullet))
			{
				bullet.X = agent.X;
				fireball = new Rectangle(850, 380, 30, 30);
			}

			if (Dragon.Intersects(bullet))
			{
				dragonLife--;
				bullet = new Rectangle(850, 380, 30, 30);
			}

			if (agent.Intersects(helthMushroom1) || agent.Intersects(helthMushroom2))
			{
				lifeBucket++;
				helthMushroom1 = new Rectangle(500, 10, 40, 40);
				helthMushroom2 = new Rectangle(700, 20, 40, 40);
			}

			if (lifeBucket == 10)
			{
				agentLife++;
				lifeBucket = 0;
			}

			if (agent.Intersects(fireball))
			{
				agentLife--;
			}
			if (agent.Intersects(poisonMush1) || agent.Intersects(poisonMush2) ||agent.Intersects(poisonMus3))
			{
				agentLife--;
				playerRunningSpeed = 0;
				poisonMush1 = new Rectangle(300, 30, 40, 40);
				poisonMush2 = new Rectangle(400, 30, 40, 40);
				poisonMus3 = new Rectangle(600, 30, 40, 40);
			}

			if (agentLife <= 0) {
				gameState = "Over";
			}
			if (dragonLife <= 0){
					gameState = "Won";
			} 

			base.Update (gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.CornflowerBlue);
		
			//TODO: Add your drawing code here
            
			if (gameState == "Start")
			{
				spriteBatch.Begin();
				spriteBatch.Draw(startScreen, new Vector2(0, 0),Color.White);
				spriteBatch.End();
			}


			if (gameState == "Running")
			{
				spriteBatch.Begin();
				spriteBatch.Draw(backgroundTex, background, Color.White);
				spriteBatch.Draw(bulletTex, bullet, Color.White); 
				spriteBatch.Draw(agentTex, agent, Color.White); 
				spriteBatch.Draw(DragonTex, Dragon, Color.White); 
				spriteBatch.Draw(fireballTex, fireball, Color.White);
				spriteBatch.Draw(healthMushroomTex, helthMushroom1, Color.White);
				spriteBatch.Draw(healthMushroomTex, helthMushroom2,Color.White);
				spriteBatch.End();
			}

			if (gameState == "Won")
			{
				spriteBatch.Begin();
				spriteBatch.Draw(youWonScreen, new Vector2(0, 0),Color.White);
				spriteBatch.End();

			}

			if (gameState == "Over")
			{
				spriteBatch.Begin();
				spriteBatch.Draw(gameOverScreen, new Vector2(0, 0),Color.White);
				spriteBatch.End();
			}
		
			base.Draw (gameTime);
		}
	}
}

