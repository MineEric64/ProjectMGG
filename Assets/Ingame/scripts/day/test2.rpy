# The scenario of Stellarhouse tests
define player = Character("[playername]", color="#f2f2f2")
define haeun = Character("이하은", color="#ffd9e9")
define yunseo = Character("조윤서", color="#aaaaaa")

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

image home day = "$/images/bg_home_day_demo.png"

transform yunseo_center:
    zoom 0.38
    xcenter 0.5
    ycenter 0.6

label start:
    # scene home day
    # show yunseo eating2 at yunseo_center
    player "그나저나 많이 배고팠어?"
    yunseo "우음, 응, 진ㅡ진짜 배고팠어."
    "그러다 밥을 한 번 곱씹어 먹은 뒤에, 말을 다시 이어나갔다."
    # show yunseo default1 at yunseo_center
    yunseo "오늘만큼은 교회에 안 가는 날이거든."
    player "응?"
    # 생각 전에 말이 먼저 튀어나와 살짝 당황한 윤서
    # show yunseo embarrassed1 at yunseo_center
    yunseo "이, 이걸 어떻게 설명해야 할지······."
    "순간, 시선이 아래로 향하는 윤서."
    "계속해서 멍한 표정인 걸 보니, 생각에 잠긴 듯하다."
    player "무슨 일이길래?"
    # 점차 차가워지는 조명, White Balance 조정하면 될 듯
    # N.C.
    yunseo "······."
    "다시 시작된 침묵."
    "아까까지만 해도 수수한 분위기로 가득 찼던 이곳은 지금,"
    "윤서가 툭 내뱉은 한마디로 공기가 급격히 조용해졌다."
    "···그래도 다행인 건, 지금 윤서의 눈빛이 이전과는 다르게 보인다는 것."
    "기분 탓이라고 하기엔··· 처음 만날 때와는 확연히 달라 보이는데."
    "윤서는 이 침묵을 깨고 싶어하는 걸까?"
    # N.C.
    yunseo "사, 사실··· 돈이 없어서 밥 머, 먹으려고 교회 다녀······."
    yunseo "밖에 나가는 건 죽어도 싫지만······ 교회는 밥 먹는 게 공짜라서······."
    "알고 보면 윤서는, 말하지 못할 사정이 많았던 걸지도 모른다."
    "왜 말 꺼내길 망설였는지··· 이제야 알 것 같네."
    yunseo "괘, 괜히 말했나···? 나 좀 이상한 사람이라서, 미, 미안해······."
    player "미안해 할 필요 없어, 돈 없는 게 너의 잘못은 아니잖아."
    player "그리고 당장 돈이 없으면 아르바이트 알아봐도 되는 거고···."
    yunseo "······."
    "윤서는 조금이라도 남아있던 기를 전부 써버린 모양이다."
    "밥 먹는데 괜히 분위기만 망친 것 같네······."
    "얼마 지나지 않아 자리에서 서서히 일어서는 윤서."
    "그런데 왜 나를 힐끔힐끔 보고 있는 걸까."
    "말로는 못 하겠지만 무언가 전하고 싶은 표정이었다."
    yunseo "···."
    "윤서는 침을 삼키곤, 천천히 손을 뻗는다."
    "···그러다 국자에서 잠깐 손이 멈춘다."
    "잠시 망설이는 게 느껴졌지만, 그래도 조심스럽게 국자를 들었다."
    "그러곤 내 그릇에 김치찌개를··· 한가득 담아준다."
    yunseo "너, 너도 많이 배고플 것 같아서···"
    yunseo "지, 짐정리하느라 마, 많이 히ㅡ힘들었지······?"
    
    menu:
        "윤서가 내 걱정을 해주다니·····":
            pass
        "응, 되게 힘들었지.":
            pass
    
    # N.C.
    # 볼이 확 빨개질 정도로 매우 수줍어 하는 윤서
    yunseo "무, 무··· 무리하지 마······."
    player "······."
    yunseo "그, 그리고··· 덕ㅡ덕분에······ 잘 먹었어···."
    yunseo "고, 고마워······ s히히s······."
    # 방문으로 들어가는 윤서, Dissolve
    # 작게 문을 닫는 SFX
    "······아까 눈빛이 다르게 보였던 건 역시, 기분 탓이 아니었어."
    player "아무래도···."
