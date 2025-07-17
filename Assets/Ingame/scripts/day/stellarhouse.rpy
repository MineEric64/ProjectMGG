# 캐릭터
define player = Character("나", color="#6d858b")
define haeun = Character("하은", color="#ffd9e9")
define yunseo = Character("윤서", color="#888888")

image haeun default = "$/images/chr_haru/default.png"
image haeun happy1 = "$/images/chr_haru/happy1.png"
image haeun happy2 = "$/images/chr_haru/happy2.png"
image haeun embarrassed1 = "$/images/chr_haru/embarrassed1.png"
image haeun embarrassed2 = "$/images/chr_haru/embarrassed2.png"
image haeun anxious = "$/images/chr_haru/anxious.png"

# 배경
image home day = "$/images/bg_home_day_demo.png"

transform haeun_t1:
    zoom 1.1
    ycenter 0.8
    xcenter 0.5

label start:
    "셰어하우스란··· 대체 어떤 곳일까."
    "그런 생각이 든 이유는 갑작스레 자취방을 비우게 됐기 때문이다."
    "전 학기까지만 해도 잘만 살고 있었는데, 집주인이 보증금 올린다나 뭐라나······."
    "그나저나, 새로운 집 비밀번호가 뭐더라?"
    "······ 0414면 내 생일인데, 이런 우연이 다 있네."
    # 비밀번호를 눌러 현관문을 여는 SFX
    # 장소: 거실 / Camera Lens Blur FX가 2초동안 서서히 사라진다
    scene home day with Dissolve(2.0)
    player "우와······."
    "확실히 집에서 누가 이미 살고 있어서 그런가, 사람 사는 느낌이 물씬 나네."
    "앞에는 TV도 있고··· 주방 쪽엔 가스레인지, 전자레인지······ 에어프라이기도 있네??"
    "그래, 이런 게 집이지. 이렇게 보니까 내 자취방은 사실 돼지우리였어."
    "그나저나, 내일이 개강이니깐 어서 짐정리나 해야지."
    "······"
    # 툭툭
    # 호기심으로 쳐다보는 하은의 캐릭터 CG
    show haeun embarrassed1 at haeun_t1 with Dissolve(0.7)
    "???" "혹시 새롭게 들어온··· 룸메?"
    "이 사람이··· 이제부터 나랑 같이 살게 될 룸메이트라고??"
    player "어··· 어어s······."
    "머릿속이 온통 새하얘진 바람에 도저히 말을 이어나가지 못 하겠다······."
    # 환하게 웃는 하은의 캐릭터 CG
    show haeun happy1 at haeun_t1
    "머리가 으깨어져 어리둥절한 내 모습을 본 건지, 살짝 미소 짓는 얼굴이 보인다."
    "???" "괜찮아요, 말 편하게 해요 ㅎㅎ"
    "처음 보는 사이인데도, 이렇게 친근하게 대해주다니······"
    "완전 착하잖아?"
    "???" "어ㅡ 하, 하은아 왔어···?"
    show haeun embarrassed1 at haeun_t1
    haeun "어? 윤서 안에 있었네?"
    "여자가······ 한 명 더 있다??"
    player "두, 둘이 아는 사이에요?"
    show haeun happy1 at haeun_t1
    haeun "네, 우리 같은 룸메에요!"
    show haeun happy2 at haeun_t1
    haeun "그리구 말 편하게 해도 된다니깐 ㅎㅎ"
    player "아, 아··· s응.s"
    "중학교 때부터 줄곧 기숙사 생활을 해왔지만"
    "룸메가 여자인 적은 처음이라 너무 당황스러워 무슨 말을 해야 할 지······"
    "······ 모르겠다."
    "아무리 생각해봐도 해결책은 떠오르지 않는다."
    show haeun embarrassed1 at haeun_t1
    haeun "무슨 생각해?"
    player "어··· 셰어하우스는 처음이라서"
    player "뭔가 되게 새로운 느낌이 드네"
    show haeun happy1 at haeun_t1
    haeun "ㅎㅎ"
    yunseo "그러엄··· 나는 머, 먼저 들어가 볼게······."
    yunseo "필요하면 불러어어······."
    # 작게 문을 닫는 SFX
    "윤서는 되게 소심한 성격을 가져 보이는 듯하다."
    "뭐, 나였어도 새로운 룸메라면 낯을 많이 가렸겠지."
    "···그것도 이성이라면 더더욱."
    # 음흉한 눈빛으로 쳐다보는 하은의 캐릭터 CG
    show haeun embarrassed2 at haeun_t1
    haeun "너 지금 윤서 보고 소심하다 생각했지!?"
    player "아, 아, 아닌데!?"
    "뭐야, 도대체 어떻게 안 거지;;"
    # 환하게 웃는 하은의 캐릭터 CG
    show haeun happy1 at haeun_t1
    "내 말을 듣곤, 곧바로 환하게 웃는 표정으로 변하는 하은."
    haeun "정말??"
    player "으응."
    "애써 부정해도 하은의 얼굴엔 의심이 좀처럼 사라지지 않는다."
    "내가 거짓말을 좀 어설프게 했나···?"
    # 무심하면서도 살짝 호기심 있게 바라보는 표정을 짓는 하은
    show haeun embarrassed1 at haeun_t1
    haeun "나는 어때 보여?"
    show haeun embarrassed2 at haeun_t1
    haeun "처음 본 사람한테 이런 말은 잘 안 하는데···"
    "대답하기 매우 어려운 질문이다."
    "마음 같아선 못 들은 척하고 넘어가고 싶지만, 하은과 시선이 맞닿아버려 쉽게 그럴 순 없어 보였다."
    "···무슨 의도가 담긴 말은 아닌 것 같은데, 이럴 땐 무슨 말을 해야 하지?"

	return

    menu:
        "잘 모르겠어.":
            "···에이, 모르겠다."
            player "잘 모르겠어."
            player "만난지 얼마 안 되기도 했고···."
            "내 말이 끝난 순간 하은의 얼굴에는 미소가 반겼다."
            haeun "아하하~ 알겠어."

        "예뻐.":
            "···에이, 모르겠다."
            player "솔직히··· 예뻐."
            "솔직하게 말하는 게 나쁜 건 아니잖아?"
            haeun "정말?"
            player "응. 동시에 조금은 무섭기도 하고···."
            "내 말이 끝난 순간 하은의 얼굴에는 미소가 반겼다."
            haeun "아하하~ 정말 솔직하게 얘기해줘서 고마워."
            haeun "그 말을 이번엔 너한테 들으니까 좋네."
            player "이번에··· 라니?"
            haeun "아하하~ 아니야."

        "그런 건 왜 물어보는 거야?":
            "···에이, 모르겠다."
            player "그런 건 왜 물어보는 거야?"
            "내 말을 듣곤 하은은 한 치의 망설임도 없이 단호하게 말을 꺼냈다."
            haeun "너라면 솔직하게 말해줄 것 같아서."
            haeun "다른 사람들은, 다 거짓말만 했거든."
            player "다른 사람들?"
            haeun "그리고 무엇보다, 네 반응이 재미있을 것 같아서."
            player "···으응?"
            "그 순간 하은의 얼굴에는 미소가 반겼다."
            haeun "아하하~ 아니야. 지금 반응도 충분히 재미있어~"

        "뭐라고 했는지 다시 말해줄 수 있어?":
            "···에이, 모르겠다. 못 들은 척하자."
            player "뭐라고 했는지 다시 말해줄 수 있어?"
            "내 말이 끝나자 단호하게 이야기하는 하은. "
            haeun "그런 건 왜 물어보는 거야?"
            player "으응?"
            haeun "아하하~ 아니야."

	"방금 웃음소리에는 무슨 뜻이 담겨져 있진 않겠지···?"
	"하은의 조용한 그 속마음은 도무지 알 수가 없다."
	"그건 그렇고, 일단 짐 정리 좀 해볼까···"
	"ㅡ라고 생각하던 찰나, 하은이 내 앞에 놓인 수많은 박스들을 보고 입을 연다."
	haeun "짐 정리 좀 도와줄까?"

	menu:
		"응, 도와줘.":
			haeun "응! 그렇게 말해줘서 고마워!"
			"내 말을 들은 순간 하은의 표정에는 활기가 가득해진다."
			"짐정리 같이 하는 게 그 정도로 좋은 건가···?"
			"뭐, 나야 좋지. 이번에 하은과 친해질 수도 있는 거고···."
			player "아니야! 나야 고맙지."

		"아니, 괜찮아.":
			haeun "알겠어···."
			"하은은 내 말을 듣곤 별 반응을 보이지 않다가······"
			"아무 말 없이 짐을 옮겨주려고 한다?"
			"난 분명히 괜찮다고 말했는데···."
			player "괜찮다니까?"
			"그러자 하은은 별 말 없이 나를 한번 보고는, 결국 짐을 들었다."
			haeun "에이, 그래도 룸메이트인데 이 정돈 해야지~"
			"뭐··· 그래."
			"두 명이서 한다고 안 좋을 건 없으니까."