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
	show seah default at half_size
	seah "ㄱㅡ그냥 제가 할게요오..."

	define hina = Character("유히나", color="#f688a7")
	hina "[playername2:야]아아!!!! 오랜만이다아!\n그동안 보고 싶었어 ㅜㅜ"

	reeverb