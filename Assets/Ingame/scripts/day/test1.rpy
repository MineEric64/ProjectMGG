# 캐릭터
define player = Character("[playername]", color="#000000")

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
    
    "윤서" "{size=-15}저{/size}... 저{size=-15}어어어어{/size}......"
    "윤서" "저기{size=-15}이이이이{/size}......"
    hina "[playername2], 이번엔 내가 할게!"

    call test_12
    call test_22

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

label test_11:
    "윤서" "어······ 여······ ㅇ,여·········"
    "윤서" "{size=-30}여, 여기서 홍보 이··· 이, 이러시며어어어언······.{/size}"
    "윤서" "{size=-15}아{/size}{size=-30}ㄴ{/size} 돼요……"
    hina "아, 된다고요? 아, 알겠습니다!"
    "안된다는 말 아니였나…?"
    "윤서" "그, 그게 아니라……"
    hina "그렇다면..."
    hina "아, 알겠다!"
    hina "점심 방송… 보러 오신 거죠?"
    "윤서" "……"
    "윤서" "(이게 아닌데… 도대체 어떻게 말해야 할지…)"
    "윤서" "(아이 모르겠다..!)"
    pause 1.5
    "윤서" "네{size=-20}에에에에{/size}……."
    return

label test_12:
    # (핵심: 윤서는 속마음이 주고, 속마음이 나올 때 캐릭터 CG 또한 같이 보여지는 것)
    "윤서" "어······ 여······ ㅇ,여·········"
    "윤서" "{size=-20}여{/size}, 여기······."
    # (속으로 부끄러워 하는 윤서의 캐릭터 CG?, >~<)
    "윤서" "(학교 가기 전에 말하는 연습 좀 하고 올 걸···!)"
    # (홍조를 띈 채 쳐다보는 윤서의 캐릭터 CG, o~o)
    "윤서" "······"
    hina "···?"
    # (눈을 감는 윤서의 캐릭터 CG)
    "윤서" "(자, 이제 갓 고딩된 조윤서. 일단 숨을 들이쉬고··· 말해보는 거야.)"
    "윤서" "(’매점에서는··· 홍보지 지양해주세요’라고···!)"
    # (N.C.)
    pause 1.5
    # (어울리는 브금: Tasty Snackbar)
    "윤서" "{size=+30}찌찌!!!!{/size}"
    # (얼굴이 새빨개진 윤서의 캐릭터 CG, o_o)
    # (몹시 당황하는 히나의 캐릭터 CG, 레퍼런스: 강여진)
    hina "네?"
    # ??? (대충 윤서의 비명? 근데 레퍼런스랑 너무 똑같은 생각이 날 수도)
    # (황급히 달려가는 윤서의 캐릭터 CG, 화면 밖으로 사라진다)
    "··· 드디어 내 귀가 맛이 갔구나."
    return

label test_21:
    hina "사령관 히나, 초콜릿 처리 완료하였습니다!"
    "학생 C" "제 초콜릿은요~?"
    "··· 어찌 보면 내 귀는 멀쩡한 걸지도."
    return

label test_22:
    hina "에··· 에ㅡ"
    hina "{size=+20}에취이이!!!{/size}"
    hina "{size=-20}아야야······{/size}"
    player "뭐하냐?"
    hina "혀 깨무어ㄹ어······."
    "··· 사실 난 멀쩡한 걸지도."
    hina "끄응······."
    "학생 C" "그래서, 제 초콜릿은요~~?"
    return