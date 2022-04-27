# TicTacToeNN
This is a WPF application themed around a neural network that can play tic-tac-toe. The app provides two main functionalities: training the neural network and playing against it yourself. The used neural network is a ![Feedforward neural network](https://en.wikipedia.org/wiki/Feedforward_neural_network) that utilizes ![Q-Learning](https://towardsdatascience.com/qrash-course-deep-q-networks-from-the-ground-up-1bbda41d3677) in its training process.
## Features
- Interactive gameplay of tic-tac-toe
- Modifying the playboard size (available size range is [3,5])
- Controlling the training process via Start, Stop and Reset buttons
- Selecting a train partner for the network (random player, boring player, other neural network), as well as the amount of training rounds
- Seeing visual statistics of the training process
## Screenshots
**Start page view**

![start page](./images/main.png)

**In-game page view**

![in-game page](./images/game_in.png)

**End-game page view**

![end-game page](./images/game_end.png)

**In-training view**

![in-training page](./images/training_in.png)

**Finished training view**

![end-training page](./images/training_end.png)

**Different playboard sizes**

![playboard size 4](./images/game_size_4.png)
![playboard size 5](./images/game_size_5.png)
## Technologies used
- .NET Framework 4.7.2
- C# 7.3
- Visual Studio 2017, refactored with Visual Studio 2022
- OxyPlot plotting library
## Acknowledgments
This project was completed as a coursework on machine learning named "Tic-Tac-Toe Neural Network" ("Нейронная сеть для игры в Крестики-Нолики"). The classes for the neural network and back propagation were based on the ![AForge.NET framework](http://www.aforgenet.com/) classes.
