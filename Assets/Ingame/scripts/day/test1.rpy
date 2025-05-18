# 캐릭터
define haru = Character("서하루", color="#a965d3")
define seah = Character("한세아", color="#3a3e4a")
define hina = Character("유히나", color="#f688a7")

image haru default = "$/images/chr_haru/default.png"
image haru happy1 = "$/images/chr_haru/happy1.png"
image haru happy2 = "$/images/chr_haru/happy2.png"
image haru embarrassed1 = "$/images/chr_haru/embarrassed1.png"
image haru embarrassed2 = "$/images/chr_haru/embarrassed2.png"
image haru anxious = "$/images/chr_haru/anxious.png"

# 배경
image classroom = "$/images/bg_classroom_demo.png"
image homebase = "$/images/bg_homebase_demo.png"

transform half_size:
    zoom 1.3
    ycenter 0.9
    xcenter 0.5

label start:
    scene classroom
    "어떡하면 좋을까."

    # scene homebase with Dissolve(1.0)
    show haru default at half_size with Dissolve(0.5)
    haru "[playername2], 반가워."
    show haru embarrassed1 at half_size
    "하루는 내게 인사를 건내는 동시에 얼굴을 붉히고 말았다."
    show haru anxious at half_size
    "왜일까."
    "..."
    show haru happy1 at half_size
    "시간이 어느정도 지나자 내게 미소를 짓는 하루."
    play music "$/audio/Nemo Neko.mp3"
    seah "ㄱㅡ그냥 제가 할게요{size=-15}오{/size}..."
    hina "[playername2:야]아아!!!! 오랜만이다{size=-10}아{/size}!\n그동안 보고 싶었어 ㅜㅜ"

    reeverb
	"그 순간 어떠한 소리도 들리지 않았다."
    seah "우으으······ {cps=*0.5}{sg=-5}미안해요오{/sg}···.{/cps}"