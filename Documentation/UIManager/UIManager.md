# UIManager

[Back to README](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/README.md)

UIManager implemented via Mediator pattern. Divided into two parts: UIManagerPlayer and UIManagerCamp/World1.

![Location of scripts](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/UIManager/uiManager01.jpg?raw=true)

1. All ui scripts are hung in the "UI Manager" and not on the UI element itself. This allows you to quickly find what you need, since it's all in one place.

2. For ui elements/gameObjects that need to be activated and deactivated, then you need to hang the script on them (Scripts > UI > UIShowHide.cs)

[Back to README](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/README.md)
