# 캐릭터
define player = Character("[playername]", color="#f2f2f2")
define haeun = Character("이하은", color="#ffd9e9")
define yunseo = Character("조윤서", color="#aaaaaa")

image haeun angry1 = "$/images/chr_haeun/angry1.png"
image haeun angry2 = "$/images/chr_haeun/angry2.png"
image haeun angry3 = "$/images/chr_haeun/angry3.png"
image haeun anxious1 = "$/images/chr_haeun/anxious1.png"
image haeun anxious2 = "$/images/chr_haeun/anxious2.png"
image haeun anxious3 = "$/images/chr_haeun/anxious3.png"
image haeun curious = "$/images/chr_haeun/curious.png"
image haeun default1 = "$/images/chr_haeun/default1.png"
image haeun default2 = "$/images/chr_haeun/default2.png"
image haeun default3 = "$/images/chr_haeun/default3.png"
image haeun default4 = "$/images/chr_haeun/default4.png"
image haeun default5 = "$/images/chr_haeun/default5.png"
image haeun disgust1 = "$/images/chr_haeun_disgust1.png"
image haeun disgust2 = "$/images/chr_haeun_disgust2.png"
image haeun happy1 = "$/images/chr_haeun/happy1.png"
image haeun happy2 = "$/images/chr_haeun/happy2.png"
image haeun happy3 = "$/images/chr_haeun/happy3.png"
image haeun happy4 = "$/images/chr_haeun/happy4.png"
image haeun happy5 = "$/images/chr_haeun/happy5.png"
image haeun happy6 = "$/images/chr_haeun/happy6.png"
image haeun happy7 = "$/images/chr_haeun/happy7.png"
image haeun happy8 = "$/images/chr_haeun/happy8.png"
image haeun happy9 = "$/images/chr_haeun/happy9.png"
image haeun happy10 = "$/images/chr_haeun/happy10.png"
image haeun yandere1 = "$/images/chr_haeun/yandere1.png"
image haeun yandere2 = "$/images/chr_haeun/yandere2.png"
image haeun yandere3 = "$/images/chr_haeun/yandere3.png"
image haeun yandere4 = "$/images/chr_haeun/yandere4.png"
image haeun yandere5 = "$/images/chr_haeun/yandere5.png"
image haeun yandere6 = "$/images/chr_haeun/yandere6.png"
image haeun yandere7 = "$/images/chr_haeun/yandere7.png"
image haeun yandere8 = "$/images/chr_haeun/yandere8.png"
image haeun yandere9 = "$/images/chr_haeun/yandere9.png"
image haeun yandere10 = "$/images/chr_haeun/yandere10.png"
image haeun yandere11 = "$/images/chr_haeun/yandere11.png"
image haeun yandere12 = "$/images/chr_haeun/yandere12.png"
image haeun yandere13 = "$/images/chr_haeun/yandere13.png"
image haeun yandere14 = "$/images/chr_haeun/yandere14.png"

image yunseo default1 = "$/images/chr_yunseo/default1.png"
image yunseo default2 = "$/images/chr_yunseo/default2.png"
image yunseo eating1 = "$/images/chr_yunseo/eating1.png"
image yunseo eating2 = "$/images/chr_yunseo/eating2.png"
image yunseo embarrassed1 = "$/images/chr_yunseo/embarrassed1.png"
image yunseo embarrassed2 = "$/images/chr_yunseo/embarrassed2.png"
image yunseo embarrassed3 = "$/images/chr_yunseo/embarrassed3.png"
image yunseo embarrassed4 = "$/images/chr_yunseo/embarrassed4.png"
image yunseo embarrassed5 = "$/images/chr_yunseo/embarrassed5.png"
image yunseo embarrassed6 = "$/images/chr_yunseo/embarrassed6.png"
image yunseo embarrassed7 = "$/images/chr_yunseo/embarrassed7.png"
image yunseo embarrassed8 = "$/images/chr_yunseo/embarrassed8.png"
image yunseo embarrassed9 = "$/images/chr_yunseo/embarrassed9.png"
image yunseo happy1 = "$/images/chr_yunseo/happy1.png"
image yunseo happy2 = "$/images/chr_yunseo/happy2.png"
image yunseo happy3 = "$/images/chr_yunseo/happy3.png"
image yunseo happy4 = "$/images/chr_yunseo/happy4.png"
image yunseo happy5 = "$/images/chr_yunseo/happy5.png"
image yunseo happy6 = "$/images/chr_yunseo/happy6.png"
image yunseo happy7 = "$/images/chr_yunseo/happy7.png"
image yunseo happy8 = "$/images/chr_yunseo/happy8.png"
image yunseo anxious1 = "$/images/chr_yunseo/anxious1.png"
image yunseo anxious2 = "$/images/chr_yunseo/anxious2.png"

# 배경
image home day = "$/images/bg_home_day_demo.png"

# 위치
transform haeun_center:
    zoom 0.43
    xcenter 0.5
    ycenter 0.66

transform haeun_left:
    zoom 0.43
    xcenter 0.35
    ycenter 0.66

transform yunseo_center:
    zoom 0.38
    xcenter 0.5
    ycenter 0.6

transform yunseo_right:
    zoom 0.38
    xcenter 0.63
    ycenter 0.6

label start:
    "셰어하우스란··· 대체 어떤 곳일까."
    "그런 생각이 든 이유는 갑작스레 자취방을 비우게 됐기 때문이다."
    "전 학기까지만 해도 잘만 살고 있었는데, 집주인이 보증금 올린다나 뭐라나······."
    "그나저나, 새로운 집 비밀번호가 뭐더라?"
    "······ 0414면 내 생일인데, 이런 우연이 다 있네."
    # 비밀번호를 눌러 현관문을 여는 SFX
    # 장소: 거실 / Camera Lens Blur FX가 2초동안 서서히 사라진다
    window hide
    scene home day with Dissolve(2.0)
    window show
    player "우와······."
    "확실히 집에서 누가 이미 살고 있어서 그런가, 사람 사는 느낌이 물씬 나네."
    "앞에는 TV도 있고··· 주방 쪽엔 가스레인지, 전자레인지······ 에어프라이기도 있네??"
    "그래, 이런 게 집이지. 이렇게 보니까 내 자취방은 사실 돼지우리였어."
    "그나저나, 내일이 개강이니깐 어서 짐정리나 해야지."
    "······{nw=1.1}"
    # 툭툭
    # 호기심으로 쳐다보는 하은의 캐릭터 CG
    show haeun curious at haeun_center with Dissolve(0.7)
    "???" "혹시 새롭게 들어온··· 룸메?"
    "이 사람이··· 이제부터 나랑 같이 살게 될 룸메이트라고??"
    player "어··· 어{size=-15}어{/size}······."
    "머릿속이 온통 새하얘진 바람에 도저히 말을 이어나가지 못 하겠다······."
    # 환하게 웃는 하은의 캐릭터 CG
    show haeun happy3 at haeun_center
    "머리가 으깨어져 어리둥절한 내 모습을 본 건지, 살짝 미소 짓는 얼굴이 보인다."
    "???" "괜찮아요, 말 편하게 해요 ㅎㅎ"
    "처음 보는 사이인데도, 이렇게 친근하게 대해주다니······"
    "완전 착하잖아?"
    show haeun happy1 at haeun_left # TODO: 캐릭터 동적 애니메이션
    show yunseo embarrassed1 at yunseo_right with Dissolve(0.4)
    "???" "어ㅡ 하, 하은아 왔어···?"
    show haeun curious at haeun_left
    haeun "어? 윤서 안에 있었네?"
    "여자가······ 한 명 더 있다??"
    player "두, 둘이 아는 사이에요?"
    show yunseo embarrassed2 at yunseo_right
    show haeun happy2 at haeun_left
    haeun "네, 우리 같은 룸메에요!"
    show haeun yandere3 at haeun_left
    haeun "그리구 말 편하게 해도 된다니깐 ㅎㅎ"
    player "아, 아··· {size=-15}응.{/size}"
    show haeun happy1 at haeun_left
    "중학교 때부터 줄곧 기숙사 생활을 해왔지만,"
    "룸메가 여자인 적은 처음이라 당황스러워 무슨 말을 해야 할 지······"
    "······ 모르겠다."
    "아무리 생각해봐도 해결책은 떠오르지 않는다."
    show haeun curious at haeun_left
    haeun "무슨 생각해?"
    player "어··· 셰어하우스는 처음이라서"
    player "뭔가 되게 새로운 느낌이 드네"
    show haeun happy1 at haeun_left
    haeun "ㅎㅎ"
    show yunseo embarrassed6 at yunseo_right
    yunseo "그러엄··· 나는 머, 먼저 들어가 볼게······."
    show yunseo embarrassed3 at yunseo_right
    yunseo "필요하면 불러어어······."
    hide yunseo with Dissolve(1.0)
    # 작게 문을 닫는 SFX
    pause 0.3
    "윤서는 되게 소심한 성격을 가져 보이는 듯하다."
    "뭐, 나였어도 새로운 룸메라면 낯을 많이 가렸겠지."
    "···그것도 이성이라면 더더욱."
    # 음흉한 눈빛으로 쳐다보는 하은의 캐릭터 CG
    show haeun yandere4 at haeun_center
    haeun "너 지금 윤서 보고 소심하다 생각했지!?"
    player "아, 아, 아닌데!?"
    "뭐야, 도대체 어떻게 안 거지;;"
    # 환하게 웃는 하은의 캐릭터 CG
    show haeun happy5 at haeun_center
    "내 말을 듣곤, 곧바로 환하게 웃는 표정으로 변하는 하은."
    haeun "정말??"
    player "으응."
    show haeun happy2 at haeun_center
    "애써 부정해도 하은의 얼굴엔 의심이 좀처럼 사라지지 않는다."
    "내가 거짓말을 좀 어설프게 했나···?"
    # 무심하면서도 살짝 호기심 있게 바라보는 표정을 짓는 하은
    show haeun curious at haeun_center
    haeun "나는 어때 보여?"
    show haeun anxious1 at haeun_center
    haeun "처음 본 사람한테 이런 말은 잘 안 하는데···"
    "대답하기 매우 어려운 질문이다."
    "마음 같아선 못 들은 척하고 넘어가고 싶지만, 하은과 시선이 맞닿아버려 쉽게 그럴 순 없어 보였다."
    "···무슨 의도가 담긴 말은 아닌 것 같은데, 이럴 땐 무슨 말을 해야 하지?"

    menu:
        "잘 모르겠어.":
            "···에이, 모르겠다."
            player "잘 모르겠어."
            player "만난지 얼마 안 되기도 했고···."
            show haeun happy1 at haeun_center
            "내 말이 끝난 순간 하은의 얼굴에는 미소가 반겼다."
            show haeun happy2 at haeun_center
            haeun "아하하~ 알겠어."

        "예뻐.":
            "···에이, 모르겠다."
            player "솔직히··· 예뻐."
            "솔직하게 말하는 게 나쁜 건 아니잖아?"
            show haeun curious at haeun_center
            haeun "정말?"
            player "응. 동시에 조금은 무섭기도 하고···."
            show haeun happy1 at haeun_center
            "내 말이 끝난 순간 하은의 얼굴에는 미소가 반겼다."
            haeun "아하하~ 정말 솔직하게 얘기해줘서 고마워."
            haeun "그 말을 이번엔 너한테 들으니까 좋네."
            player "이번에··· 라니?"
            show haeun happy2 at haeun_center
            haeun "아하하~ 아니야."

        "그런 건 왜 물어보는 거야?":
            "···에이, 모르겠다."
            player "그런 건 왜 물어보는 거야?"
            show haeun default1 at haeun_center
            "내 말을 듣곤 하은은 한 치의 망설임도 없이 단호하게 말을 꺼냈다."
            haeun "너라면 솔직하게 말해줄 것 같아서."
            haeun "다른 사람들은, 다 거짓말만 했거든."
            player "다른 사람들?"
            haeun "그리고 무엇보다, 네 반응이 재미있을 것 같아서."
            player "···으응?"
            show haeun happy2 at haeun_center
            "그 순간 하은의 얼굴에는 미소가 반겼다."
            haeun "아하하~ 아니야. 지금 반응도 충분히 재미있어~"

        "뭐라고 했는지 다시 말해줄 수 있어?":
            "···에이, 모르겠다. 못 들은 척하자."
            player "뭐라고 했는지 다시 말해줄 수 있어?"
            show haeun anxious1 at haeun_center
            "내 말이 끝나자 단호하게 이야기하는 하은. "
            show haeun default1 at haeun_center
            haeun "그런 건 왜 물어보는 거야?"
            player "으응?"
            show haeun happy2 at haeun_center
            haeun "아하하~ 아니야."

    show haeun happy1 at haeun_center
    "방금 웃음소리에는 무슨 뜻이 담겨져 있진 않겠지···?"
    "하은의 조용한 그 속마음은 도무지 알 수가 없다."
    "그건 그렇고, 일단 짐 정리 좀 해볼까···"
    "ㅡ라고 생각하던 찰나, 하은이 내 앞에 놓인 수많은 박스들을 보고 입을 연다."
    show haeun default2 at haeun_center
    haeun "짐 정리 좀 도와줄까?"

    menu:
        "응, 도와줘.":
            show haeun happy2 at haeun_center
            haeun "응! 그렇게 말해줘서 고마워!"
            "내 말을 들은 순간 하은의 표정에는 활기가 가득해진다."
            "짐정리 같이 하는 게 그 정도로 좋은 건가···?"
            "뭐, 나야 좋지. 이번에 하은과 친해질 수도 있는 거고···."
            player "아니야! 나야 고맙지."

        "아니, 괜찮아.":
            show haeun anxious1 at haeun_center
            haeun "알겠어···."
            "하은은 내 말을 듣곤 별 반응을 보이지 않다가······"
            show haeun default1 at haeun_center
            "아무 말 없이 짐을 옮겨주려고 한다?"
            "난 분명히 괜찮다고 말했는데···."
            player "괜찮다니까?"
            "그러자 하은은 별 말 없이 나를 한번 보고는, 결국 짐을 들었다."
            show haeun happy1 at haeun_center
            haeun "에이, 그래도 룸메이트인데 이 정돈 해야지~"
            "뭐··· 그래."
            "두 명이서 한다고 안 좋을 건 없으니까."

    player "그럼 하은, 너가 1호 상자좀 맡아줘. 그게 제일 작은 거니까 아무래도 들기에 더욱 수월할 거야."
    show haeun default1 at haeun_center
    haeun "맡아달란 이야기는··· 상자를 열어달란 소리지?"
    player "응."
    show haeun happy1 at haeun_center
    haeun "응! 알겠어."
    # 상자를 뜯는 SFX
    show haeun default1 at haeun_center
    pause 1.1
    "······ 그렇게 서로 조용히 짐을 정리하던 중, 문득 내 고개를 들었을 때."
    show haeun curious at haeun_center
    "하은은 유난히 호기심 있는 표정으로 상자를 바라보고 있었다."
    "그 상자에는 뭐, {cps=*0.5}특별한 건 {/cps}{cps=*0.3}딱히{/cps}{cps=*0.06}··· {/cps}{cps=*0.8}어!?{/cps}"
    "팬티가 들어있는 속옷이 들어가 있다는 걸 깜빡했다!!"
    "절대로 하은에게 보여줘선 안 돼!!"
    player "하, 하은아!! 미안한데 이 상자 말고, 다른 상자 열어줄래?!"
    show haeun default2 at haeun_center
    haeun "응? 너가 지금 들고 있는 상자 준다면야."
    player "아, 알았어···!"
    player "휴우···."
    "다행히 내 인생 최대 큰 위기는 면했다."
    "그대로 상자를 열었으면 하은이 날 뭐라고 생각했을지······."
    "하은의 말대로, 들고 있던 상자를 살며시 건네준다."
    show haeun yandere1 at haeun_center
    haeun "고마워, 너가 만진 거라 따뜻하네."
    player "아, 으응."
    show haeun default1 at haeun_center
    "내가 건네준 상자를 바라보곤 잠깐 멈칫하는 하은."
    show haeun curious at haeun_center
    haeun "이 상자는 열어도 되지?"
    player "어······ 잠시만, 생각할 시간 좀."
    window hide
    pause 1.1
    window show
    player "응, 열어도 돼."
    show haeun happy3 at haeun_center
    haeun "좋아~"
    "이제 상자 뜯는 거에 능숙해진 하은은 재빠르게 테이프를 뜯어 상자를 연다."
    show haeun curious at haeun_center
    "그러다 내가 가져온 목걸이를 보곤 놀란 토끼 눈으로 나를 지그시 바라본다."
    "어서 이야기를 꺼내고 싶다는 듯이···."
    haeun "오? 너도 목걸이 써?"
    player "목걸이 예전에 샀는데, 지금은 잘 안 써."
    "언제부터 장식용으로 전락했을까··· 그래도 그때 꽤 주고 산 것 같은데."
    show haeun default2 at haeun_center
    haeun "그러면 혹시 내가 당분간 써도 될까?"
    player "너 지금 목걸이 쓰고 있는 거 아니야?"
    show haeun anxious2 at haeun_center
    haeun "아, 이거······."
    "내 말에 잠자코 있던 하은은 나의 알록달록한 목걸이를 흘겨보곤 다시 말을 이어나갔다."
    show haeun default1 at haeun_center
    haeun "사실 목걸이는 많으면 많을 수록 좋거든."
    "내 목걸이에 무슨 대단한 뜻이라도 있는 건가?"
    player "뭐, 알겠어."
    "아예 주는 것도 아니고 당분간 빌려 쓴다고 했으니까."
    show haeun yandere1 at haeun_center
    haeun "응! [playername2:야] 고마워!"
    "무척 해맑은 눈으로 미소를 짓는 하은."
    "하은이 이렇게 진심으로 웃는 모습은 만나서 처음으로 보는 듯하다."
    "하은은, 목걸이를 정말로 좋아하는구나."
    show haeun yandere2 at haeun_center
    haeun "다른 사람들한텐 안 그러지? 나한테만 주는 거지?"
    player "어, 애초에 그 목걸이 언젠가 처분하려고 했어."
    show haeun happy3 at haeun_center
    haeun "아하하~ 좋아."
    haeun "그리고 이 물건은 첫 번째 서랍에 넣어둘게~"
    player "응."

    menu:
        "****여기서, 하은의 분량을 더 넣을 것인가?"
        
        "파스 이벤트를 넣는다: 잡담을 대학생활로":
            "분량 매우 늘어남"

        "파스 이벤트를 넣는다: 잡담을 목걸이로":
            "분량 살짝 늘어남"

        "파스 이벤트를 넣지 않는다":
            "분량 굿, 하지만 하은의 배경을 소화하지 못함"

    scene home day with Fade(1.0, 1.0, 1.0)
    "짐정리를 드디어 끝냈다"
    show haeun happy1 at haeun_center with Dissolve(0.8)
    haeun "[playername2] 고생 많았어~ 난 수업 듣고 다시 올게!"
    # 현관문이 닫히고 도어락이 작동하는 SFX
    hide haeun with Dissolve(0.8)
    # 꼬르륵 SFX
    pause 1.1
    player "아··· 배고프다."
    "하은과 같이 짐정리를 끝내자마자 내 뱃속에서 수신호가 오네."
    "아까 꺼내놓은 OO 먹어야겠다."
    # 요리 SFX
    "노릇노릇 익는다?"
    # 작은 방문이 열리는 SFX
    # 배고픈데 먹을 것을 발견한 윤서, 입에 침이 고여있고 초롱해진 눈으로 헤벌레 웃는 윤서의 캐릭터 CG
    show yunseo happy7 at yunseo_center with Dissolve(0.4)
    player "어··· 윤서?"
    show yunseo happy3 at yunseo_center
    yunseo "으응······ 나, 나도··· 배고파서··· 헤헤······."
    "냄새가 윤서 방까지 스며 들어간 건가."
    "······"
    show yunseo happy1 at yunseo_center
    "자꾸만 옆에서 무슨 인기척이 느껴진다."
    "윤서인가 본데, 내가 노릇노릇 구운 OO을 쳐다보고 있는 걸까."
    "···아니면 나를 뚫어지게 바라보고 있는 걸까."
    "후자라면 살짝 부담스럽긴 한데···."
    # 꼬르륵 SFX
    show yunseo embarrassed5 at yunseo_center
    "(꼬르륵...)"
    # 살짝 당황하는 윤서의 캐릭터 CG
    "······진심으로 배고팠구나."
    show yunseo embarrassed4 at yunseo_center
    player "거의 다 끝났어, 이제 계란만 넣으면······"
    player "끝!"
    # 다시 헤벌레 웃는 표정을 짓는 윤서의 캐릭터 CG
    show yunseo happy5 at yunseo_center
    yunseo "으헤헤······"
    "먼저 냄비를 들어 올리고···{p}식탁에 놓기 위해서는 냄비받침이 필요한데···."
    show yunseo default1 at yunseo_center
    "문제는 식탁을 세팅할 손이 남지 않는다는 것."
    # 그저 멍하니 바라보는 윤서의 캐릭터 CG
    pause 1.5
    "아, 윤서가 있었지."
    player "저기 윤서, 냄비받침이랑 식탁좀 세팅해 줄 수 있어? 손이 안 남아서 말이야."
    # 깜빡 잊고 있었다는 듯이 살짝 놀라는 윤서의 캐릭터 CG
    show yunseo embarrassed5 at yunseo_center
    yunseo "아! 으응."
    show yunseo embarrassed8 at yunseo_center
    "내 말을 듣곤 그제서야 분주하게 움직이는 윤서."
    "···뭐 냄비받침이 바로 눈 앞에 있어서 ‘분주하게’라고 말할 것까진 아니지만."
    # 둥툭한 SFX 
    show yunseo embarrassed2 at yunseo_center
    "좋아, 윤서가 준 냄비받침에다 둔 뒤에 이제 먹으면······ 어?"
    "접시랑 수저가 없네;;"
    "분명히 식탁 세팅좀 해달라고 말했던 것 같은데···"
    player "윤서?"
    show yunseo default2 at yunseo_center
    yunseo "으, 으응?"
    player "······"
    # N.C.
    show yunseo embarrassed7 at yunseo_center
    yunseo "······."
    "아무 말 없이 냄비만 멀뚱멀뚱 쳐다보는 윤서."
    player "밥 안 먹어?"
    show yunseo anxious at yunseo_center
    yunseo "먹, 먹어야 되는데··· 나한테 수저가 없어서······."
    player "어······ 집에 수저 있지 않나??"
    show yunseo embarrassed3 at yunseo_center
    yunseo "집밥은 오, 오랜만이라서··· 어디있는지 잘······"
    show yunseo embarrassed4 at yunseo_center
    yunseo "이럴 땐 보, 보통 나눠주긴 하거든······."
    player "어······"
    # N.C.
    show yunseo embarrassed7 at yunseo_center
    yunseo "······."
    "언제까지 이러고 있을 순 없지···."
    player "어··· 그냥 내가 접시랑 수저 갖고 올게."
    show yunseo embarrassed5 at yunseo_center
    yunseo "아, 아!··· 으응."
    show yunseo embarrassed4 at yunseo_center
    # 쇠끼리 부딪치는 SFX
    player "어디 있으려나···."
    # 수저통 그림 등장
    "어?"
    player "바로 앞에 있는데?"
    show yunseo embarrassed5 at yunseo_center
    yunseo "저, 정말···?"
    player "응, 못 봤구나."
    show yunseo happy1 at yunseo_center
    "수저와 접시를 챙기자, 손에 얹어달라는 듯이 양손을 공손히 모으는 윤서."
    "그런 윤서의 손에 수저를 먼저 살며시 내려놓고, 접시는 앞에 놓아준다."
    "···이런 것까지 겸손할 필요는 없는데."
    show yunseo embarrassed2 at yunseo_center
    yunseo "···."
    "수저를 받자, 눈을 깜빡이며 잠깐 멈칫하는 윤서.{p}그러곤 밥을 한 입, 두 입··· 조심스럽게 먹기 시작했다."
    show yunseo eating2 at yunseo_center
    player "그럼, 잘 먹겠습니다."
    yunseo "······."
    "내 말에 아랑곳하지 않고 밥에만 시선이 향해 있는 윤서."
    show yunseo default1 at yunseo_center
    "그러다 젓가락을 가볍게 내려놓더니, 얼굴을 들고 나를 또렷한 눈빛으로 바라본다."
    "무슨 말이라도 하고 싶은 걸까?"
    "얼마 지나지 않아 윤서가 살짝 미소를 지은 채로 처음으로 먼저 입을 열어주었다."
    show yunseo happy2 at yunseo_center
    yunseo "맛, 맛있어···"
    player "진짜로?"
    show yunseo happy5 at yunseo_center
    yunseo "으응, 콩나물국만 먹다 와서 그런가······ 진, 진짜 맛있다 이거."
    "내가 만든 밥을 이렇게 맛있다고 말해준 사람은, 윤서가 처음이다."
    "지금까지 나는 요리와는 전혀 안 맞는다고 생각했는데······."
    player "칭찬해줘서 고마워."
