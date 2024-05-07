<header>
  <h3>Flow Runner</h3>
  <p class="tagline">SE 3330 - Spring 2024</p>
</header>
<hr>

**Team Name**: Team Waterfall

**Team Members**: Cade VanHout, Kase Tadych, Ryan Traut, Zach Landquist

<details><summary><b>Setting Up Unity Instructions</b>:</summary>

1. Clone this repo
2. Download [Unity LTS Release 2022.3.19f1](https://unity.com/releases/editor/qa/lts-releases?version=2022.3#:~:text=Released%3A%20January%2031%2C%202024)
3. Download [Unity Hub](https://unity.com/download)
4. Provison a Student License using [this form](https://unity.com/products/unity-student)
5. Open Unity Hub
6. Add license received from email (Unity Technologies <accounts@unity3d.com>)
7. Click dropdown arrow next to 'Add'

![Unity Hub Buttons](https://i.imgur.com/7uHeAIS.png)

8. Click 'Add project from disk'
9. Locate 'Flow Runner' folder inside root of this repo on your local device. Unity will then open the project and setup required files, this takes some time.
10. In the project tree, open 'Scenes' and then 'Title Screen'

![Project Tree](https://i.imgur.com/wDERqbS.png)

11. Double click on 'Title Screen' scene

![Title Screen Scene](https://i.imgur.com/AgP67lG.png)

12. Click play button at top of screen

![Play Button](https://i.imgur.com/oNYSdn8.png)

13. Play the game
</details>

<details><summary><b>Build Instructions</b>:</summary>

1. Go to File > Build Settings

![Build Settings](https://i.imgur.com/lBujpoC.jpeg)

2. Make sure all 3 Scences are added. If not, click the Add Open Scenes button and add them.

![Add Scenes](https://i.imgur.com/FGfKMzo.jpeg)

3. Click Build at the bottom and selcet a location for the build to go. (This should be a folder for the game)

</details>

<details><summary><b>Code Coverage Instructions</b>:</summary>

Note: the last coverage report can be found without having to run a new report, skip to step 4 to open it.

1. Go to Window > Analysis > Code Coverage

2. Run test runner

3. Click Generate Report

4. Navigate to "src\Flow Runner\CodeCoverage\Report\index.htm" and open

5. Website loads with the coverage report

</details>

<details><summary><b>Library Information</b>:</summary>
Unity LTS Release 2022.3.19f1
</details>

Known bugs:
- Platforms can sometimes spawn back-to-back forcing to player to potentially drown.
