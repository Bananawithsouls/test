import os
import requests
import subprocess
import winreg

# Constants
GAME_NAME = "Goose Goose Duck"
REG_FILE_URL = "https://drive.google.com/uc?export=download&id=18Yr6wfSAJZTqhttMFVDNx7pZkez2vJBq"
REG_FILE_NAME = "settings.reg"
STEAM_EXE = "steam.exe"
GAME_EXE = f"{GAME_NAME}.exe"


def find_game_folder():
    dir_path = 'C:\\'
    for root, dirs, files in os.walk(dir_path):
        if "Steam" in dirs:
            return os.path.join(root, "Steam", "steamapps", "common", GAME_NAME)
    return None


def download_reg_file(url, destination_folder):
    response = requests.get(url)
    if response.status_code == 200:
        file_path = os.path.join(destination_folder, REG_FILE_NAME)
        os.makedirs(destination_folder, exist_ok=True)
        with open(file_path, 'wb') as f:
            f.write(response.content)
            print("Файл реестра скачан.")
        return file_path
    else:
        print(f"Ошибка при загрузке файла реестра: {response.status_code}")
        return None


def import_reg_file(reg_file_path):
    try:
        key = winreg.OpenKey(winreg.HKEY_CURRENT_USER, r"SoftwareValveSteamsteamglobal", 0, winreg.KEY_ALL_ACCESS)

        with open(reg_file_path, "r") as file:
            content = file.read()
            winreg.SetValueEx(key, f"AppVolume_{GAME_NAME}", 0, winreg.REG_SZ, content)

        winreg.CloseKey(key)
        os.remove(reg_file_path)  
        print("Файл реестра успешно импортирован.")
    except Exception as e:
        print(f"Ошибка при импорте файла реестра: {e}")


def launch_game(game_folder):
    steam_exe_path = os.path.join(os.path.dirname(game_folder), STEAM_EXE)
    game_exe_path = os.path.join(game_folder, GAME_EXE)

    if os.path.exists(steam_exe_path):
        subprocess.Popen(steam_exe_path, creationflags=subprocess.CREATE_NEW_CONSOLE)
        print("Запуск Steam...")
    elif os.path.exists(game_exe_path):
        subprocess.Popen(game_exe_path, creationflags=subprocess.CREATE_NEW_CONSOLE)
        print("Запуск игры...")
    else:
        print("Не удалось найти исполняемые файлы Steam или игры.")


if __name__ == "__main__":
    game_folder = find_game_folder()
    if game_folder:
        reg_file_path = download_reg_file(REG_FILE_URL, game_folder)
        if reg_file_path:
            import_reg_file(reg_file_path)
        launch_game(game_folder)
    else:
        print(f"Не удалось найти папку установки {GAME_NAME}.")
