# 캐릭터
define player = Character("[playername]", color="#f2f2f2")
define haeun = Character("이하은", color="#ffd9e9")
define yunseo = Character("조윤서", color="#aaaaaa")

image haeun angry1 = "images/chr_haeun/angry1.png"
image haeun angry2 = "images/chr_haeun/angry2.png"
image haeun angry3 = "images/chr_haeun/angry3.png"
image haeun anxious1 = "images/chr_haeun/anxious1.png"
image haeun anxious2 = "images/chr_haeun/anxious2.png"
image haeun anxious3 = "images/chr_haeun/anxious3.png"
image haeun curious = "images/chr_haeun/curious.png"
image haeun default1 = "images/chr_haeun/default1.png"
image haeun default2 = "images/chr_haeun/default2.png"
image haeun default3 = "images/chr_haeun/default3.png"
image haeun default4 = "images/chr_haeun/default4.png"
image haeun default5 = "images/chr_haeun/default5.png"
image haeun disgust1 = "images/chr_haeun/disgust1.png"
image haeun disgust2 = "images/chr_haeun/disgust2.png"
image haeun happy1 = "images/chr_haeun/happy1.png"
image haeun happy2 = "images/chr_haeun/happy2.png"
image haeun happy3 = "images/chr_haeun/happy3.png"
image haeun happy4 = "images/chr_haeun/happy4.png"
image haeun happy5 = "images/chr_haeun/happy5.png"
image haeun happy6 = "images/chr_haeun/happy6.png"
image haeun happy7 = "images/chr_haeun/happy7.png"
image haeun happy8 = "images/chr_haeun/happy8.png"
image haeun happy9 = "images/chr_haeun/happy9.png"
image haeun happy10 = "images/chr_haeun/happy10.png"
image haeun yandere1 = "images/chr_haeun/yandere1.png"
image haeun yandere2 = "images/chr_haeun/yandere2.png"
image haeun yandere3 = "images/chr_haeun/yandere3.png"
image haeun yandere4 = "images/chr_haeun/yandere4.png"
image haeun yandere5 = "images/chr_haeun/yandere5.png"
image haeun yandere6 = "images/chr_haeun/yandere6.png"
image haeun yandere7 = "images/chr_haeun/yandere7.png"
image haeun yandere8 = "images/chr_haeun/yandere8.png"
image haeun yandere9 = "images/chr_haeun/yandere9.png"
image haeun yandere10 = "images/chr_haeun/yandere10.png"
image haeun yandere11 = "images/chr_haeun/yandere11.png"
image haeun yandere12 = "images/chr_haeun/yandere12.png"
image haeun yandere13 = "images/chr_haeun/yandere13.png"
image haeun yandere14 = "images/chr_haeun/yandere14.png"

image yunseo default1 = "images/chr_yunseo/default1.png"
image yunseo default2 = "images/chr_yunseo/default2.png"
# image yunseo eating1 = "images/chr_yunseo/eating1.png"
image yunseo eating2 = "images/chr_yunseo/eating2.png"
image yunseo embarrassed1 = "images/chr_yunseo/embarrassed1.png"
image yunseo embarrassed2 = "images/chr_yunseo/embarrassed2.png"
image yunseo embarrassed3 = "images/chr_yunseo/embarrassed3.png"
image yunseo embarrassed4 = "images/chr_yunseo/embarrassed4.png"
image yunseo embarrassed5 = "images/chr_yunseo/embarrassed5.png"
image yunseo embarrassed6 = "images/chr_yunseo/embarrassed6.png"
image yunseo embarrassed7 = "images/chr_yunseo/embarrassed7.png"
image yunseo embarrassed8 = "images/chr_yunseo/embarrassed8.png"
# image yunseo embarrassed9 = "images/chr_yunseo/embarrassed9.png"
image yunseo happy1 = "images/chr_yunseo/happy1.png"
image yunseo happy2 = "images/chr_yunseo/happy2.png"
image yunseo happy3 = "images/chr_yunseo/happy3.png"
image yunseo happy4 = "images/chr_yunseo/happy4.png"
image yunseo happy5 = "images/chr_yunseo/happy5.png"
# image yunseo happy6 = "images/chr_yunseo/happy6.png"
image yunseo happy7 = "images/chr_yunseo/happy7.png"
# image yunseo happy8 = "images/chr_yunseo/happy8.png"
# image yunseo anxious1 = "images/chr_yunseo/anxious1.png"
# image yunseo anxious2 = "images/chr_yunseo/anxious2.png"

image yunseo a1 = "images/chr_yunseo/a1.png"
image yunseo a6 = "images/chr_yunseo/a6.png"
image yunseo a7 = "images/chr_yunseo/a7.png"
image yunseo embarrassed1_blush = "images/chr_yunseo/embarrassed1_blush.png"
image yunseo embarrassed2_blush = "images/chr_yunseo/embarrassed2_blush.png"
image yunseo embarrassed3_blush = "images/chr_yunseo/embarrassed3_blush.png"
image yunseo embarrassed4_blush = "images/chr_yunseo/embarrassed4_blush.png"
image yunseo happy5_blush = "images/chr_yunseo/happy5_blush.png"
image yunseo happy7_blush = "images/chr_yunseo/happy7_blush.png"
image yunseo embarrassed52 = "images/chr_yunseo/embarrassed52.png"
image yunseo embarrassed53 = "images/chr_yunseo/embarrassed53.png"
image yunseo happysuper2 = "images/chr_yunseo/happysuper2.png"

# 배경
image black = Solid("#000000")

image home day = "images/bg_home_day_demo.png"
image home night = "images/bg_home_night_demo.png"

# 위치
transform haeun_center:
    zoom 0.46
    xcenter 0.5
    ycenter 0.7

transform haeun_left:
    zoom 0.46
    xcenter 0.35
    ycenter 0.7

transform yunseo_center:
    zoom 0.8
    xcenter 0.5
    ycenter 0.6

transform yunseo_right:
    zoom 0.8
    xcenter 0.63
    ycenter 0.6

transform yunseo_nc:
    zoom 0.34
    xanchor 1.0
    yanchor 1.0
    xpos 0.42
    ypos 0.3

label start:
    "셰어하우스란··· 대체 어떤 곳일까."
    "그런 생각이 든 이유는 갑작스레 자취방을 비우게 됐기 때문이다."
    "전 학기까지만 해도 잘만 살고 있었는데, 집주인이 보증금 올린다나 뭐라나······."
    "결국 다른 곳을 이리저리 알아보던 중에 셰어하우스가 유독 눈에 들어왔다."
    "가격이 워낙 싸서 별로 기대는 안 하고 있지만··· 룸메이트만큼은 제발 좋은 분이었으면."
    window hide
    # 짐을 내려놓는 SFX
    pause 1.1
    window show
    player "새로운 집 비밀번호가 뭐였더라······ 0915?"
    "비밀번호가 마침 내 생일이랑 같네, 우연치곤 신기하다."
    # 비밀번호를 눌러 현관문을 여는 SFX
    # 장소: 거실 / Camera Lens Blur FX + Lens Flare FX가 2초 동안 서서히 사라진다
    window hide
    scene home day with Dissolve(2.0)
    window show
    player "우와······."
    "생활용품들이 가지런히 정돈된 거실."
    "따뜻한 조명과 정돈된 가구들이 만들어내는 아늑한 공기."
    "게다가 창문 너머로 스며드는 아련한 햇살까지."
    "순간 분위기에 압도되어 그저 앞을 멍하니 바라볼 수밖에 없었다."
    player "여기서 살게 되는구나···."

    "······{nw=1.1}"
    # 툭툭
    play music ROOMIE_ROOKIE
    # 호기심으로 쳐다보는 하은의 캐릭터 CG
    show haeun curious at haeun_center with Dissolve(0.7)
    "{color=#ffd9e9}???" "혹시 새롭게 들어온··· 룸메?"
    "이 사람이··· 이제부터 나랑 같이 살게 될 룸메이트라고??"
    player "{color=#cccccc}{size=-10}(존나 예쁘잖아?){/size}{/color}"
    "{color=#ffd9e9}???" "저기···?"
    player "어··· 어{size=-15}어{/size}······."
    # 환하게 웃는 하은의 캐릭터 CG
    show haeun happy3 at haeun_center
    "머릿속이 새하얘진 내 모습을 본 건지, 살짝 미소 짓는 얼굴이 보인다."
    "{color=#ffd9e9}???" "괜찮아요, 말 편하게 해요!"
    "처음 보는 사이인데도, 이렇게 친근하게 대해주다니······"
    "완전 착하잖아!"
    show haeun happy1 at haeun_left # TODO: 캐릭터 동적 애니메이션
    show yunseo embarrassed1 at yunseo_right with Dissolve(0.4)
    "{color=#aaaaaa}???" "어ㅡ 하, 하은아 왔어···?"
    show haeun curious at haeun_left
    haeun "어? 윤서 안에 있었네?"
    "여자가······ 한 명 더 있다??"
    player "두, 둘이 아는 사이에요?"
    show haeun happy2 at haeun_left
    haeun "네, 우리 같은 룸메에요!"
    show yunseo embarrassed2 at yunseo_right
    show haeun yandere3 at haeun_left
    haeun "그리구 말 편하게 해도 된다니깐 ㅎㅎ"
    player "아, 아··· {size=-15}응.{/size}"
    show haeun happy1 at haeun_left
    "고등학교 때 기숙사 생활을 오래 해봤지만,"
    "룸메이트가 여자인 적은 처음이라 무슨 말을 해야 할지······"
    "······ 모르겠다."
    "아무리 생각해 봐도 해결책은 떠오르지 않는다."
    show haeun curious at haeun_left
    haeun "무슨 생각해?"
    player "어··· 셰어하우스는 처음이라서"
    player "뭔가 되게 새로운 느낌이 드네"
    show haeun happy1 at haeun_left
    "그저 말없이 환하게 웃는 하은."

    show yunseo embarrassed6 at yunseo_right
    yunseo "그러엄··· 나는 머, 먼저 들어가 볼게······."
    show yunseo embarrassed3 at yunseo_right
    yunseo "필요하면 불러{size=-10}어어{/size}······."
    # 소심하게 한 걸음씩 오른쪽으로 내디디며 사라지는 윤서
    hide yunseo with Dissolve(1.0)
    # 작게 문을 닫는 SFX
    pause 0.3
    "윤서는 되게 소심한 성격을 가져 보이는 듯하다."
    "뭐, 새로운 룸메이트라면 낯을 많이 가리는 게 당연하지."
    "···그것도 이성이라면 더더욱."
    # 음흉한 눈빛으로 쳐다보는 하은의 캐릭터 CG
    show haeun yandere4 at haeun_center
    haeun "너 지금 윤서 보고 소심하다고 생각했지!?"
    player "아, 아, 아닌데!?"
    # 환하게 웃으면서도 씨익 웃는 하은의 캐릭터 CG
    show haeun happy5 at haeun_center
    "곧바로 씨익 웃는 표정으로 변하는 하은."
    haeun "정말??"
    player "으응."
    show haeun happy2 at haeun_center
    "아무래도 내 반응을 재미있어하는 듯한 모양이다."
    "내가 거짓말을 좀 어설프게 했나···?"
    # 무심하면서도 살짝 호기심 있게 바라보는 표정을 짓는 하은
    show haeun curious at haeun_center
    haeun "나는 어때 보여?"
    show haeun anxious1 at haeun_center
    haeun "처음 본 사람한테 이런 말은 잘 안 하는데···"
    "하은이가 내게 의미심장한 질문을 던졌다."
    "마음 같아선 못 들은 척하고 넘어가고 싶지만, 하은과 시선이 맞닿아버려 쉽게 그럴 순 없어 보였다."
    "···무슨 의도가 담긴 말은 아닌 것 같은데, 이럴 땐 무슨 말을 해야 하지?"

    menu:
        "잘 모르겠어.":
            "···에이, 모르겠다."
            player "잘 모르겠어."
            player "만난 지 얼마 안 되기도 했고···."
            show haeun happy1 at haeun_center
            "내 말이 끝난 순간 하은이의 얼굴에는 미소가 반겼다."
            show haeun happy2 at haeun_center
            haeun "아하하~ 알겠어."

        "예뻐.":
            # 하은이가 좋아할 만한 선택지, 호감도 +1
            "···솔직하게 말하는 게 나쁜 건 아니잖아?"
            player "···예뻐."
            show haeun curious at haeun_center
            haeun "정말?"
            player "응. 동시에 조금은 무섭기도 하고···."
            show haeun happy1 at haeun_center
            "내 말이 끝난 순간 하은이의 얼굴에는 미소가 반겼다."
            haeun "아하하~ 너 꽤 귀여운 구석이 있구나?"
            haeun "그 말을 이번엔 너한테 들으니까 좋네."
            player "이번에··· 라니?"
            show haeun happy2 at haeun_center
            haeun "아하하~ 아니야."

        "그런 건 왜······.":
            "···에이, 모르겠다."
            player "그런 건 왜······."
            # 무심하면서도 싸늘한 표정으로 곧바로 바뀌는 하은의 캐릭터 CG
            show haeun default1 at haeun_center
            "내 말을 듣곤 하은이는 한 치의 망설임도 없이 단호하게 말을 꺼냈다."
            haeun "그냥. 너라면 솔직하게 말해줄 것 같아서."
            # 경멸한 표정을 짓는 하은의 캐릭터 CG, 얀데레 특유 표정 like [얀데레 경멸]
            # 경멸할 때, 시선이 아래를 향하는 것도 나쁘지 않다. 적용해 보고 이상하면 폐기
            haeun "다른 사람들은, 다 거짓말만 했거든."
            player "다른 사람들?"
            # 다시 무심하면서도 호기심 있게 바라보는 하은의 캐릭터 CG로 바뀐다
            haeun "그리고 무엇보다, 네 반응이 재미있을 것 같아서."
            player "···으응?"
            show haeun happy2 at haeun_center
            "그 순간 하은이의 얼굴에는 미소가 반겼다."
            haeun "아하하~ 농담이야!"

        "응? 뭐라고?":
            # 호감도 -1
            "···이럴 땐 못 들은 척하는 게 가장 나을지도 모르겠다."
            player "응? 뭐라고?"
            show haeun anxious1 at haeun_center
            "내 말이 끝나자 단호하게 이야기하는 하은."
            # 경멸한 표정을 짓는 하은의 캐릭터 CG, 얀데레 특유 표정 like [얀데레 경멸]
            show haeun default1 at haeun_center
            haeun "그런 건 왜 물어보는 거야?"
            player "으응?"
            "···그런 눈으로 바라보니 하은이가 조금 무서워졌다."
            # 무심하면서도 호기심 있게 바라보는 하은의 캐릭터 CG로 바뀐다

    "그때, 하은이는 내 앞에 놓인 수많은 박스를 보고 말을 이어나간다."
    show haeun happy1 at haeun_center
    haeun "이 박스들 다 짐이야?"
    "맞다, 짐 정리한다는 걸 완전히 잊어먹고 있었지."
    player "응, 이제 정리하려고."
    "일단 작은 것부터 시작해볼까ㅡ라고 생각하던 찰나."
    show haeun default2 at haeun_center
    haeun "짐 정리 좀 도와줄까?"

    menu:
        "응, 도와줘.":
            # 하은이가 좋아할 만한 선택지, 호감도 +1
            show haeun happy2 at haeun_center
            haeun "응! 그렇게 말해줘서 고마워!"
            "그 순간 하은이의 표정에는 활기가 가득해졌다."
            "뭐, 나도 이번 기회에 하은이와 친해질 수 있어서 좋긴 하지만···"
            "보통 이런 건 내가 고마워해야 하는 거 아닌가···?"
            player "아니야, 나야 고맙지."

        "아니, 괜찮아.":
            show haeun anxious1 at haeun_center
            haeun "알겠어···."
            "하은이는 내 말을 듣곤 별 반응을 보이지 않다가······"
            show haeun default1 at haeun_center
            "아무 말 없이 짐을 옮겨주려고 한다?"
            "난 분명히 괜찮다고 말했는데···."
            player "괜찮다니까?"
            "그러자 하은이는 별말 없이 나를 한번 보고는, 결국 짐을 들었다."
            show haeun happy1 at haeun_center
            haeun "에이, 그래도 룸메이트인데 이 정돈 해야지~"
            "뭐··· 그래."
            "둘이서 한다고 안 좋을 건 없으니까."

    player "그럼 하은아, 네가 1호 상자 좀 맡아줘."
    player "그게 제일 작은 거니까 아무래도 들기에 더욱 수월할 거야."
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
    "하은이는 유난히 호기심 있는 표정으로 상자를 바라보고 있었다."
    "그 상자에는 뭐, {cps=*0.5}특별한 건 {/cps}{cps=*0.3}딱히{/cps}{cps=*0.06}··· {/cps}{cps=*0.8}{size=+25}어!?{/size}{/cps}"
    # TODO?: Music Speed up
    "팬티가 들어있는 속옷이 들어가 있다는 걸 깜빡했다!!"
    "절대로 하은이에게 보여줘선 안 돼!!"
    player "하, 하은아!! 미안한데 이 상자 말고, 다른 상자 열어줄래?!"
    show haeun default2 at haeun_center
    haeun "응? 네가 지금 들고 있는 상자 준다면야."
    player "아, 알았어···!"
    player "휴우···."
    "다행히 내 인생 최대로 큰 위기는 면했다."
    "그대로 상자를 열었으면 하은이가 날 뭐라고 생각했을지······."
    "하은이의 말대로, 들고 있던 상자를 살며시 건네준다."
    
    show haeun yandere1 at haeun_center
    haeun "고마워, 네가 만진 거라 따뜻하네."
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
    "이제 상자 뜯는 거에 능숙해진 하은이는 재빠르게 테이프를 뜯어 상자를 연다."
    show haeun curious at haeun_center
    "내가 가져온 목걸이를 보자 놀란 토끼 눈으로 나를 지그시 바라본다."
    "어서 이야기를 꺼내고 싶다는 듯이···."
    haeun "오? 목걸이네?"
    player "목걸이 예전에 좀 썼다가, 지금은 잘 안 써."
    "언제부터 장식용으로 전락했을까··· 그래도 그때 꽤 걸고 다녔던 것 같은데."
    show haeun default2 at haeun_center
    haeun "그러면 혹시 내가 당분간 써도 될까?"

    menu:
        "왜?":
            show haeun anxious2 at haeun_center
            # TODO?: Music Speed down
            # 눈이 내려가 자기 목걸이를 보는 하은의 캐릭터 CG, 이상하면 폐기
            window hide
            pause 1.5
            window show
            "잠시 얼어있던 하은이는 내 유리 목걸이를 슬쩍 쳐다본다."
            show haeun default1 at haeun_center
            haeun "이 목걸이, {size=-1}예전부터{/size} 한번 써보고 싶었거든."
            haeun "유리라서 더욱 의미 있을 것 같기도 하고."

        "너 지금 목걸이 쓰고 있는 거 아니야?":
            show haeun anxious2 at haeun_center
            # TODO?: Music Speed down
            # 눈이 내려가 자기 목걸이를 보는 하은의 캐릭터 CG, 이상하면 폐기
            haeun "아, 이거······."
            "내 말에 잠자코 있던 하은이는 내 유리 목걸이를 흘겨보곤 다시 말을 이어나갔다."
            show haeun default1 at haeun_center
            haeun "사실··· 목걸이는 {size=-1}많으면 많을수록{/size} 좋거든."
    
    "내 목걸이에 무슨 대단한 뜻이라도 있는 건가?"
    player "뭐, 알겠어."
    "아예 주는 것도 아니고 당분간 빌려 쓴다고 했으니까."
    show haeun yandere1 at haeun_center
    haeun "응! [playername2:야] 고마워!"
    "무척 해맑은 눈으로 미소를 짓는 하은."
    "하은이가 이렇게 진심으로 웃는 모습은 만나서 처음으로 보는 듯하다."
    "하은이는, 목걸이를 정말로 좋아하는구나."
    show haeun yandere2 at haeun_center
    haeun "다른 사람들한텐 안 그러지? 나한테만 주는 거지?"
    player "어, 애초에 그 목걸이 언젠가 처분하려고 했어."
    show haeun happy3 at haeun_center
    haeun "아하하~ 좋아."
    haeun "그리고 생필품은 첫 번째 서랍에 넣어둘게~"
    player "응."

    stop music fadeout 3.0
    scene home day with Fade(1.0, 2.0, 2.0)
    "···그렇게 어제 온종일 짐 정리만 하고 잤다."
    "같이 정리해서 망정이지, 하은이가 없었으면 선반 조립은 어떻게 했으려나."
    # 꼬르륵 SFX
    pause 1.1
    player "아··· 너무 배고프다."
    "남은 것 중에 큰 건 대충 끝났으니까, 일단 밥 좀 먹어야겠다."
    player "오랜만에 김치찌개 해 먹어야지."
    "자취할 때 자주 먹었던 터라, 찌개 요리는 그 누구보다 자신이 있었다."
    window hide
    # 요리 SFX
    pause 0.5
    window show
    "일단··· 물을 1컵 정도 부어주고······."
    "그다음에 레시피에선 볶은 김치 넣으라고 하네, 원래 순서가 이게 맞나?"
    # 작은 방문이 열리는 SFX
    # 배고픈데 먹을 것을 발견한 윤서, 주위에서 계속 어슬렁거린다
    play music STARBERRY_MILK
    show yunseo a1 at yunseo_center with Dissolve(0.4)
    player "어··· 윤서?"
    show yunseo happy3 at yunseo_center
    yunseo "으응······ 나, 나도··· 배고파서··· 헤헤······."
    "냄새가 윤서 방까지 스며 들어간 건가."
    show yunseo happy1 at yunseo_center
    window hide
    # 요리 SFX
    pause 1.0
    # 입에 침이 고여있고 초롱초롱해진 눈으로 헤벌레 웃는 윤서의 캐릭터 CG
    show yunseo a1 at yunseo_center
    pause 0.5
    window show
    "찌개를 휘저을 때마다··· 옆에서 자꾸만 인기척이 느껴진다."
    "윤서는, 내가 보글보글 끓인 김치찌개를 쳐다보고 있는 걸까."
    "···아니면 나를 뚫어지게 바라보고 있는 걸까."
    "후자라면 살짝 부담스럽긴 한데···."
    # 꼬르륵 SFX
    show yunseo embarrassed5 at yunseo_center
    "(꼬르륵...)"
    # 살짝 당황하는 윤서의 캐릭터 CG
    "······진심으로 배고팠구나."
    show yunseo embarrassed4 at yunseo_center
    player "윤서, 김치찌개 같이 먹어."
    show yunseo embarrassed52 at yunseo_center
    yunseo "지, 진짜···?"
    player "응, 거의 다 끝났어. 이제 소금만 뿌려주면······"
    player "끝!"

    # 다시 헤벌레 웃는 표정을 짓는 윤서의 캐릭터 CG
    show yunseo happysuper2 at yunseo_center
    yunseo "으헤헤······"
    "먼저 냄비를 들어 올리고···{w} 식탁에 놓기 위해서는 냄비받침이 필요한데···."
    show yunseo default1 at yunseo_center
    "문제는 식탁을 세팅할 손이 남지 않는다는 것."
    # 그저 멍하니 바라보는 윤서의 캐릭터 CG
    pause 1.5
    "아, 윤서가 있었지."
    player "저기, 냄비받침이랑 식탁 좀 세팅해 줄 수 있어?\n손이 안 남아서 말이야."
    # 깜빡 잊고 있었다는 듯이 살짝 놀라는 윤서의 캐릭터 CG
    show yunseo embarrassed5 at yunseo_center
    yunseo "아! 으응."
    show yunseo embarrassed8 at yunseo_center
    "내 말을 듣곤 그제야 분주하게 움직이는 윤서."
    "···뭐 냄비받침이 바로 눈앞에 있어서 ‘분주하게’라고 말할 것까진 아니지만."
    # 뭉툭한 SFX
    show yunseo embarrassed2 at yunseo_center
    "좋아, 윤서가 준 냄비받침에다 둔 뒤에 이제 먹으면······ 어라?"
    "정작 냄비를 뜰 수저가 없네."
    "분명히 식탁 세팅 좀 해달라고 말했던 것 같은데···"
    player "윤서?"
    show yunseo default2 at yunseo_center
    yunseo "으, 으응?"
    show yunseo embarrassed2 at yunseo_center
    window hide
    # TODO?: SFX 구현할 때 N.C.도 SFX 넣는 게 어울릴지 고민해보기
    fx NC at yunseo_nc
    pause 2.5 hard
    window show
    show yunseo embarrassed7 at yunseo_center
    yunseo "······."
    "아무 말 없이 냄비만 멀뚱멀뚱 쳐다보는 윤서."
    player "밥 안 먹어?"
    yunseo "먹, 먹어야 하는데··· 나한테 수저가 없어서······."
    player "집에 수저 있지 않아?"
    show yunseo embarrassed3 at yunseo_center
    yunseo "어··· 집밥은 오, 오랜만이라서··· 어디 있는지 잘······"
    show yunseo embarrassed4 at yunseo_center
    yunseo "이럴 땐 보, 보통 나눠주긴 하거든······."
    window hide
    fx NC at yunseo_nc
    pause 2.2 hard
    window show
    show yunseo embarrassed7 at yunseo_center
    yunseo "······."
    "언제까지 이러고 있을 순 없지···."
    player "어··· 그냥 내가 접시랑 수저 갖고 올게."
    show yunseo embarrassed5 at yunseo_center
    yunseo "아, 아!··· 으응."

    show yunseo embarrassed4 at yunseo_center
    player "어디 있으려나···."
    window hide
    pause 1.1
    window show
    # 쇠끼리 부딪치는 SFX
    player "찾았다, 바로 앞에 있네."
    show yunseo embarrassed5 at yunseo_center
    yunseo "저, 정말···?"
    show yunseo embarrassed3 at yunseo_center
    yunseo "{size=-10}미{/size}, {size=-10}미안해{/size}···."
    show yunseo happy1 at yunseo_center
    "수저와 접시를 챙기자, 손에 얹어 달라는 듯 양손을 공손히 모으는 윤서."
    "그런 윤서의 손에 수저를 먼저 살며시 내려놓고, 접시는 앞에 놓아준다."
    "···그나저나 그렇게까지 겸손할 필요는 없는데."
    show yunseo embarrassed2 at yunseo_center
    yunseo "······."
    "수저를 받자, 눈을 깜빡이며 잠깐 멈칫하는 윤서.{p}그러곤 밥을 한 입, 두 입··· 조심스럽게 먹기 시작했다."
    show yunseo eating2 at yunseo_center
    player "그럼 잘 먹겠습니다."
    "내 말에 아랑곳하지 않고 밥에만 시선이 향해 있는 윤서."
    show yunseo default1 at yunseo_center
    "그러다 젓가락을 가볍게 내려놓더니, 얼굴을 들고 나를 또렷한 눈빛으로 바라본다."
    "무슨 말이라도 하고 싶은 걸까?"
    "얼마 지나지 않아 윤서가 살짝 미소를 지은 채로 처음으로 먼저 입을 열어주었다."
    show yunseo happy2 at yunseo_center
    yunseo "맛, 맛있어···"
    player "진짜로?"
    show yunseo happy5 at yunseo_center
    yunseo "으응, 콩나물국만 먹다 와서 그런가··· 진, 진짜 맛있다 이거."
    "내가 만든 밥을 이렇게 맛있다고 말해준 사람은, 윤서가 처음이다."
    "아무리 요리에 자신 있다고는 해도, 혹여나 입맛에 안 맞을까 봐 걱정했는데···."
    player "칭찬해 줘서 고마워."
	
    show yunseo eating2 at yunseo_center
    player "그나저나 많이 배고팠어?"
    yunseo "우음, 응, 진ㅡ진짜 배고팠어."
    "그러다 밥을 한 번 곱씹어 먹은 뒤에 말을 다시 이어나갔다."
    show yunseo default1 at yunseo_center
    yunseo "오늘만큼은 교회 쉬는 날이거든."
    window hide
    pause 1.0
    # TODO?: Music Speed down
    # 생각 전에 말이 먼저 튀어나와 순간 당황한 윤서의 캐릭터 CG / 윤서 특유 불안 표정 + o_o
    show yunseo embarrassed53 at yunseo_center
    window show
    "순간, 윤서의 하얀 얼굴이 붉게 달아오른다."
    player "응?"
    # 삐졌어요 그리고 놀랐어요 표정 / 소스라치게 놀라는 모습 참고 
    show yunseo a6 at yunseo_center
    yunseo "아, {sg=*0.93}그, 그ㅡ그게······.{/sg}"
    show yunseo embarrassed3_blush at yunseo_center
    "윤서의 말끝이 흐려졌다."
    player "무슨 일이길래?" # or 뭐라고?
    # 점차 차가워지는 조명, White Balance 조정하면 될 듯
    window hide
    fx LC_FRAME at yunseo_nc
    pause 2.2 hard
    window show
    show yunseo embarrassed1_blush at yunseo_center
    # TODO: 이 중간에 embarrassed1_blush로 대체하고, 기존은 시선을 아래로 향하자. (조윤서 문서 참고)
    yunseo "······."
    "아무래도 무슨 생각에 잠긴 듯한 모양이다."
    player "괜찮아, 천천히 이야기해도 돼."
    "하지만 붉게 물든 윤서의 얼굴은 좀처럼 사라지지 않았다."
    "그런데 왜 나를 힐끔힐끔 보고 있는 걸까."
    "말로는 못 하겠지만 무언가 전하고 싶은 표정이었다."
    show yunseo embarrassed2_blush at yunseo_center
    "이런 어색한 분위기 속 윤서는 서서히 자리에서 일어선다."
    show yunseo embarrassed4_blush at yunseo_center
    yunseo "···."
    "윤서는 침을 삼키곤, 천천히 손을 뻗는다."
    "···그러다 국자에서 잠깐 손이 멈춘다."
    show yunseo embarrassed8 at yunseo_center
    "잠시 망설이는 게 느껴졌지만, 결심이 섰는지 결국 국자를 뜬다."
    # 다시 천천히 따뜻해지는 조명
    show yunseo embarrassed2 at yunseo_center
    "그러곤 내 그릇에 김치찌개를··· 한가득 담아준다."
    show yunseo happy3 at yunseo_center
    yunseo "{size=-5}미,{/size} 미안··· 내가 할 수 있는 게 이것밖에 없어서······."
    yunseo "너도 많이 배고팠을 텐데···."
    show yunseo happy4 at yunseo_center
    yunseo "지, 짐 정리하느라 많이 힘들었지······?"

    menu:
        "(윤서가 내 걱정해주다니······)":
            pass
        "응, 되게 힘들었지.":
            pass

    # 볼이 확 빨개질 정도로 매우 수줍어하는 윤서
    show yunseo a7 at yunseo_center
    yunseo "무ㅡ무리하지 마······."
    "예상치 못한 배려에 순간 말문이 막혔다."
    show yunseo happy7_blush at yunseo_center
    yunseo "그, 그리고 덕분에 잘 먹었어······."
    show yunseo happy5_blush at yunseo_center
    yunseo "고마워······ {size=-10}으헤헤{/size}······."
    window hide
    # 방문으로 들어가는 윤서, Dissolve
    hide yunseo with Dissolve(1.5)
    # 작게 문을 닫는 SFX
    pause 3.0
    window show
    player "갑자기 왜 김치찌개에 단맛이 나는 거지······."
    stop music fadeout 3.0

    scene black with Fade(2.0, 2.0, 1.5)
    # 소리가 크지만 뭉툭하게 난다, 마치 ‘쿵쿵쿵!’ 비스무리 SFX
    player "어우······."
    "밥 먹고 그대로 쭉 자버리고 말았다."
    player "아침부터 너무 무리한 탓인가, 몸이 아직도 뻐근하네···."
    # 아까와 같지만 작게 나는 SFX
    player "일단 일어난 김에 물 좀 마셔야겠다."
    "침대에서 일어나 곧장 거실로 발걸음을 옮겼다."
    window hide
    pause 1.5
    scene home night with Dissolve(2.5) # TODO: 시간대가 새벽인 거실 배경
    pause 1.0
    window show
    "이렇게 어두운 거 보니 한창 새벽인 모양이야."
    # 이번엔 크게 나는 SFX, 4~8초마다 주기적으로 한 번씩 SFX 재생, 랜덤 이용
    "······그나저나 아까부터 이게 무슨 소리지?"
    # 화면이 흔들리며, wiggle FX
    "???" "으악!"
    "무언가 넘어지는 소리와 함께 짧은 비명이 났다."
    "아무래도 누가 다친 게 분명하다."
    "일단 소리가 방 안에서 난 건 확실한데, 중요한 문제가 있었다."
    "이게 하은이 방에서 들린 건지, 아니면 윤서 방인지 모르겠다는 것···."
    "이대로 무시하기엔 위험한 상황 같은데··· 누구를 먼저 확인해야 하지······?"

    menu:
        "하은이 방을 확인한다.":
            pass

        "윤서 방을 확인한다.":
            pass

    return