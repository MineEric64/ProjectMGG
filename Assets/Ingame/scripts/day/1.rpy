# 캐릭터
define player = Character("[playername]", color="#6d858b")
define yunseul = Character("윤슬", color="#3a3e4a")

image yunseul default = "$/images/chr_haru/default.png"

# 배경
image home day = "$/images/bg_home_day_demo.png"
image home night = "$/images/bg_home_night_demo.png"

image school gate = "$/images/bg_school_gate_demo.png"

transform yunseul_t1:
    zoom 1.1
    ycenter 0.8
    xcenter 0.2

label start:
    # 장소: 집 / 시간: 새벽 4시
    scene home night
	player "아······. 하루라도 빨리 아싸로부터 탈출하고 싶다."
	"유두브 알고리즘으로 우연히 뜬 아싸 브이로그 영상을 보자마자 든 생각이었다."
	"이 기만자들."
	"진짜 아싸는 방구석에 박힌 채 폰질만 하면서 이불에서 벗어나질 않는데."
	"…"
	"생각해 보니, 초등학교 때 같이 놀던 친구 한 명쯤은 있었던 것 같다."
	"그것도 여자아이."
	"어느 순간부터 보이지 않아 그대로 연락이 끊기긴 했지만."
	"… 뭐, 잘 지내겠지."
	# 비 오는 SFX + 번개 FX
	"사실, 내가 불편해서 일부러 연락을 끊은 건가···?"
	player "……"
	"슬프다."
	"그나마 다행인 건, 학교는 남고가 아닌 남녀공학이라는 것."
	"학교만 가도 여자가 주변에 있다는 게 얼마나 행복한 일일까."
	"그런 행복한 상상을 할 때마다 나는 인싸로 성격 탈바꿈해서 지내보고 싶어진다."
	"그런데 그러기 위해선 뭐부터 해야 하지."
	# 번개 FX
	"아, 그래."
	player "그냥 애니 좀 보고 잠이나 자야지"
	# 시간: 오전 7시 50분, 즉 지각 직전
    scene home day with Fade(1.0, 1.0, 1.0)
	# 알람 소리 SFX, TODO: 알람 소리를 무엇으로 할지는 미지정
	"오늘따라 유독 시끄럽게 울리는 알람 소리에 몸이 저절로 깨어났다."
	"창가를 보니, 이른 아침이라고 하기엔 화려한 햇살이 나를 온통 감쌌다."
	"… 지금 몇 시지?"
	# 스마트폰을 꺼내 시간을 확인하는 남주
	"지각이구나."
	player "ㅅㅂ"
	"하··· 입학식 날부터 지각이라니."
	"첫인상부터 대차게 말아먹게 생겼는데 뭐? 인싸???"
	"에휴."

	scene school gate with Fade(1.5, 1.0, 1.0)
	"깔끔하게 정각 도착은 포기했다."
	"학교를 들어가려던 찰나, 뒤에서 들리는 소리에 무심코 뒤돌아보니···"
	show yunseul default at yunseul_t1 with Dissolve(0.7)
	# 빠르게 뛰고 있는 윤슬 캐릭터 CG 등장
	# 잠깐 나를 흘깃 보더니 다시 쓩 뛰어가서 화면에서 사라지는 윤슬
	player "와······."
	"왜 저 여자애는 혼자 해상도 다르게 사는 걸까."
	"갸름하고 이쁜 눈에 가느다란 다리가 순간 나를 홀렸다."
	"첫날부터 이렇게 예쁜 여자애를 보다니···"
	player "ㅎㅎ"