# 캐릭터
define player = Character("[playername]", color="#000000")

define seah = Character("한세아", color="#3a3e4a")
image seah default = "$/images/chr_seah/1.png"
image seah happy1 = "$/images/chr_seah/4.png"
image seah happy2 = "$/images/chr_seah/5.png"
image seah blush1 = "$/images/chr_seah/2-1-1.png"
image seah blush2 = "$/images/chr_seah/2-1-2.png"
image seah blush3 = "$/images/chr_seah/2-1-3.png"
image seah blush4 = "$/images/chr_seah/2-2-1.png"
image seah blush5 = "$/images/chr_seah/2-2-2.png"
image seah blush6 = "$/images/chr_seah/2-2-3.png"
image seah blush7 = "$/images/chr_seah/2extend.png"
image seah blush8 = "$/images/chr_seah/3.png"

define hina = Character("유히나", color="#f688a7")
image hina = "$/images/chr_hina_demo.png"

transform seah_t1:
	zoom 0.7
	xalign 0.7
	yalign 0.6

transform hina_t1:
	zoom 1.5
	xalign 0.3
	yalign 0.65

# 배경
image classroom = "$/images/bg_classroom_demo.png"

label start:
	scene classroom
	"2교시부터 쭉 겨울잠처럼 자던 몸이 무의식적으로 반응해 일어나게 되었다."
	"일어나자마자 서둘러 시계를 보니, 시곗바늘은 지금이 점심시간이라고 알려주고 있었다."
	"한창 점심시간이라 교실은 휑한 채 불만 켜져있고, 아무도 없었다."
	"윤슬도 없었다."
	"보이는 건 오직 창문에서 비쳐 보이는 푸른 하늘과 텅 비어있는 운동장뿐."
	"지금 바로 달려간다 해도 급식줄이 빼곡하게 쌓여 있을 게 뻔하겠지."
	"…"
	"에이 그냥 안 먹어야겠다."
	"애초에 급식을 빨리 먹기엔 내가 너무 늦게 일어나기도 했고."
	"(드…르륵)"
	play music "$/audio/Nemo Neko.mp3"
	show seah blush7 at seah_t1
	seah "저기이이,, [playername2] 맞으시죠···?"
	player "네."
	seah "… 방송부 합격하셨어요."
	seah "알려드릴 게 있으니 밥 먹고 방송실로 와주세요···!"
	"방송부를 합격했다는 게 믿기지가 않는다."
	"진심으로 방송하고 싶어서 지원한 게 아닌 사람인 내가 무려 방송부원이라니."
	"무엇보다 면접할 때 말을 절어서 중간에 반쯤 포기한 듯 아무말 대잔치했는데···."
	show seah blush8 at seah_t1
	seah "저… 그리고······."
	show seah blush4 at seah_t1
	seah "저ㅡ저 방송부 선배인 한세아라고 하니깐 앞으로 잘 부탁해요오!!!"
	reeverb
	"선배의 쩌렁쩌렁한 목소리가 교실에 울려펴졌다."
	show seah blush5 at seah_t1
	"소리가 얼마나 컸는지 그 뒤로는 평소에 들리던 소리도 들리지 않을 정도로."
	"새가 창가에 앉아 편안하게 지저귀는 소리도, 더군다나 바람에 커튼이 흔들리는 소리도,"
	"지금 이 순간만큼은 시간이 멈춰버린 것만 같이 아주 조용했다."
	show seah blush8 at seah_t1
	"세아 선배는 이제 말을 어떻게 이어나가야 할지 모르겠다는 기색으로 내 눈을 힐끔 피하고 있는다."
	"선배가 그러고 있으면 나ㅡ나는 어떻게 해야···."
	"……"
	"몇 초가 지났을까."
	"서로 어색하게 흐르고 있던 침묵을 깰 무언가가 필요할 마침, 교실 문이 힘차게 열린다."
	"드르륵… 쿵!"
	play music "$/audio/Nemo Neko.mp3"
	show hina at hina_t1
	hina "[playername2:야]!!"
	hina "너 방송부 합격했엉!!! 몰랐지?"
	player "알고 있었는데."
	player "뒷북 치네 ㅋ"
	# 귀여운 사람이 귀엽게 화낼 때 쓰는 표정을 하는 히나의 캐릭터 CG, ><랑 ㅡㅡ 합친거? + 구슬요 마카오톡 표정
	"반쯤 삐진 듯하다."
	"하지만 히나는 어떻게든 내 말에 지지 않으려 한다."
	player "장난이야 ㅋㅋㅋㅋ"
	hina "뒷북 아닌 거 하나 더 있거든??"
	hina "이건 너 절대 모를 거야!"
	player "으음··· 너도 방송부 합격한 거?"
	# 놀라워 하는 히나의 캐릭터 CG, 역전재판 2에 나오는 여자아이 참고
	"히나는 깜짝 놀란 나머지 갸름한 눈을 크게 뜨며 입을 손으로 막았다."
	hina "너 독심술사 맞지??"
	player "아니야 ㅋㅋㅋㅋ"
	player "뭔가 그럴 것 같아서 찍어 맞춘 거야."
	"이게 맞네?"
	player "내가 눈치 하나는 끝내주지 ㅎ"
	# 날 보며 경멸하는 히나의 캐릭터 CG 등장
	"내 장난에 제대로 먹혔는지 히나의 얼굴은 점차 찌푸려진다."
	"오히려 좋아."
	hina "그런데 옆에는.. 누구?"
	show seah blush7 at seah_t1
	seah "아 저,, 저는······"
	player "방송부 선배인 한세아 누나셔."
	show seah blush2 at seah_t1
	"뇌에 필터링을 거치지 않은 채 생각없이 누나라고 말이 나와버렸다."
	"아무래도 히나가 있어서 히나의 평소 장난치는 성격을 의식한 듯하다."
	"방금 말실수로 인해 세아 선배가 싫어하지는 않을까."
	"선배의 얼굴을 보니 아니나 다를까, 그 자리에서 꽁꽁 얼어붙었다."
	"얼굴이 빨갛게 물들어간 채로 말이다."
	hina "헉 세아 선배님 정말 안녕하세요!! 저 방송부 신입인 유히나라고 합니당!"
	hina "잘 부탁드려요오~~!!"
	"히나 특유의 열렬한 성격대로, 히나는 열정적으로 선배를 환영했다."
	hina "[playername2:야] 뭐해!! 언능 인사드려야지!"
	player "아.. 안녕하세요 세아 선배님! 저도 히나랑 같은 방송부 신입입니다."
	player "잘 부탁드려요."
	show seah default at seah_t1
	seah "…"
	# 눈은 보이지 않지만 웃는 표정인 세아의 캐릭터 CG
	"세아 선배는 우리의 인삿말에도 잠자코 있었다."
	"하지만 표정을 대략 보니 세아 선배 나름대로 우리 둘을 환영하고 있는 듯하다."
	hina "세아 선배님! 제가 제안 하나 해봐도 될까요!?"
	seah "…"
	hina "선배님이 부장하시고 저는 차장 하는 거 어때요?"
	hina "일단 전 찬성!"
	"갑작스러운 건 둘째 치고, 너무 이른 시간에 정하는 거 아닌가 싶었지만 그것보다 더 중요한 문제가 있었다."
	"내 자리는 도대체 어디 있는 거야."
	player "나는···?"
	hina "[playername2:는] 차장인 내가 특별히! 세컨드 차장으로 임명해줄게 ㅎㅎ"
	seah ".. 고민은 좀 해봐야겠지만, 일단 임시로 그렇게 하는게 좋을 것 같아요···."
	# 눈빛이 초롱초롱한 히나의 캐릭터 CG
	hina "우와앙!!!"
	hina "역시 선배님! 믿고 있었다구요!!"
	"우리는 세컨드 차장을 그저 방송부원이라고 부르기로 했어요…"
	"(꼬르륵…)"
	show seah blush7 at seah_t1
	"귀가 빨개진 세아 선배."
	hina "… 저희 일단 밥 먹으러 가시죠!"
	"그렇게 셋이서 밥을 먹으러 급식실로 내려갔다."
