define seah = Character("한세아", color="#3a3e4a")
image seah default = "$/images/chr_seah/1.png"
image classroom = "$/images/bg_classroom_demo.png"

transform half_size:
    zoom 0.5
    ycenter 0.7
    xcenter 0.5

label start:
    scene classroom
    "어떡하면 좋을까."
    seah "..."
    "히나" "안녕~"
    show seah default at half_size with Fade(0.5, 0, 0.5)
    seah "안녕하세요오.."
    play music "$/audio/Nemo Neko.mp3"
    pause 1
    seah "ㄱㅡ그냥 제가 할게요오..."
    seah "그ㅡ그냥 {fast}제가 할게요..."

    define hina = Character("유히나", color="#f688a7")
    hina "[playername2:야]아아!!!! 오랜만이다아!\n그동안 보고 싶었어 ㅜㅜ"

    reeverb
	"그 순간 어떠한 소리도 들리지 않았다."