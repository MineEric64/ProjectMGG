 define player = Character("[playername]", color="#000000")

define seah = Character("한세아", color="#abb7db")
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

transform seah_t1:
	zoom 0.41
	xcenter 0.5
	ycenter 0.6

image homebase = "$/images/bg_homebase_demo.png"

image cg_seah1 = "$/images/cg_demo.png"

label start:
	scene homebase
	show seah default at seah_t1
	play music "$/audio/Adorkable Love v3.mp3"
	seah "여기가 홈베이스에요."
	seah "홈베이스 벽면 쪽에 우리 포스터를 붙이면 될 것 같아요."
	"세아 선배는 포스터를 붙이러 홈베이스 안으로 들어갔다."
	"나도 뒤따라 홈베이스 안으로 들어갔다."
	"한창 점심시간이라 그런지 학생들은 주위에 별로 없었다."
	"특히나 지금 홈베이스에는 아무도 없다."
	"창문이 열려있어 차가운 바람이 살살 불어온다."
	player "어우 무거워."
	"나는 들고 있던 포스터 전부를 바닥에 내려놓았다."
	"어차피 여기서 포스터를 붙일 거니까."
	"그럼 이제 포스터를 붙여볼까?"
	"포스터 하나를 집자 세아 선배는 단호한 목소리로 반응했다."
	seah "포스터 붙이는 건 제가 할게요."
	show seah blush7 at seah_t1
	seah "더군다나 아까 포스터를 다 들어준 건 고마웠어요···."
	player "하하···"
	"세아 선배에게 칭찬을 들으니 어깨가 조금이나마 으쓱해졌다."
	show seah default at seah_t1
	"선배는 곧바로 포스터 하나를 집어 ‘어디에 붙여야지 포스터가 제일 잘 보일까?’라는 듯이 망설이고 있었다."
	"얼마 지나지 않아 세아 선배는 결심이 선 듯 자리를 약간 옆으로 옮겼다."
	"그리고 선배는 포스터를 붙이기 위해 양손을 위로 쭉 뻗는다."
	"세아 선배가 끙끙대며 까치발을 드는 모습은··· 너무 귀엽잖아."
	"하지만 세아 선배의 몸이 조금씩 휘청거리기 시작한다."
	"한번 잘못 삐끗하면 헛디뎌 넘어질 것만 같았다."
	"…… 아니나 다를까, 내 예측은 벗어나지 않았다."
	# 세아를 붙잡으며 무척 당황한 세아의 CG
	scene cg_seah1 with dissolve
	"세아 선배가 균형을 잃어 넘어지는 그 순간, 나는 선배를 반사적으로 잡았다."
	"선배가 바로 내 앞에 있어서 망정이지, 만약 한 발짝 더 멀리 있었다면 넘어지던 선배를 놓치고 말았을 것이다."
	"세아 선배는 그대로 몸이 꽁꽁 얼어 붙어있었다."
	player "선배님 괜찮으세요···?"
	seah "……"
	"내가 말을 꺼내도 달라지는 건 없었다."
	"여전히 나는 언짢게 세아 선배를 품에 안기고 있는다."
	"지금 이 상황에서 어떻게 해야 할지 모르겠다는 눈빛으로 나를 흘끗 보더니, 민망한 듯 다시 시선을 피한다."
	"그와 반대로 나는 세아 선배의 당황한 표정에 이끌리듯 선배를 지그시 쳐다보고만 있는다."
	"그런 내 시선을 의식했기 때문인지, 세아 선배의 새빨갛게 물든 얼굴이 좀처럼 사라지지 않는 모양이다."
	"…"
	"시간이 조금 흘러, 세아 선배는 얼어 붙어있던 몸을 풀으려 안간힘을 다하기 시작했다."
	"아무래도 포스터를 다시 붙이려고 일어나려는 거겠지."
	"아쉽다. 세아 선배와 계속 가까이 있고 싶었는데."
	scene homebase with dissolve
	show seah blush7 at seah_t1
	"세아 선배는 넘어지면서 내동댕이 친 포스터를 다시 주웠다."
	"발을 동동 구르던 와중에 나를 물끄러미 바라본다."
	"나에게 무슨 할 말이 있는 걸까?"
	"세아 선배는 조심스러워 하는 기색으로 말을 꺼냈다."
	show seah blush2 at seah_t1
	seah "저··· 포스터 같이 붙이면 안될까요오···?"
	show seah blush8 at seah_t1
	seah "저 혼자 포스터를 붙이다가··· 아까처럼 넘어지며언······."
	player "음.. 알겠어요 선배님. 그럼 이건 어때요?"
	player "제가 포스터 위쪽을 붙일 테니, 선배님은 아래쪽을 붙이는 거죠."
	show seah happy1 at seah_t1
	seah "오! 그건 생각 못했네요 ㅎ.ㅎ"
	show seah happy2 at seah_t1
	seah "고마워요 [playername2]씨!"
	"세아 선배는 그동안 본 적이 없던 밝은 미소를 내게 처음 지었다."
	"뭐랄까, 마음 속에서 뒤섞인 감정이 함께 우러나오는 이 기분···."
	"이상하다."
	play music "$/audio/Sweet Haru.mp3"
	# 포스터를 잡으면서 세아가 나를 위로 쳐다보며 밝은 표정을 짓는 구도인 CG 등장
	show seah default at seah_t1
	"세아 선배는 주머니에서 커터칼과 테이프를 꺼내 내게 건냈다."
	player "제가 포스터를 들게요."
	"포스터가 제법 무거웠기에 그냥 내가 포스터를 드는 게 낫다고 생각했다."
	"나는 곧바로 포스터를 들고 포스터가 잘 보일만한 위치로 옮겼다."
	player "포스터 위치 어때요? 괜찮나요?"
	seah "포스터를 조금만 더··· 옆으로 붙여야 할 것 같아요."
	player "… 이정도로요?"
	seah "좀만 더···."
	player "됐죠?"
	show seah happy1 at seah_t1
	seah "좋아요! 그대로 가만히 냅두면 돼요."
	seah "제가 그동안 테이프로 붙이고 있을게요."
	"세아 선배는 테이프를 쫘악 뜯다 문득 작은 목소리로 이야기를 꺼냈다."
	show seah blush7 at seah_t1
	seah "혹시 [playername2]씨는, 취미가 뭐에요···?"
	"첫 이야기 중에서도 하필 대답하기 어려운 질문이라니···."
	player "아, 저 애니메이션을 보면서 감상···하는 게 제 취미에요."
	"애니 본다는 게 부끄러운 건 아니잖아?"
	show seah default at seah_t1
	seah "오! 저 애니메이션 하나 본 거 있어요."
	seah "아실려나···?"
	"오!? 애니를 봤다고??"
	"도대체 무슨 애니일지 괜히 궁금해지는데."
	player "뭔데요?!"
	seah "장구는 못말려라고, 최근에 나온 거 봤는데 재밌더라구요."
	player "아······."
	player "그럼 선배님은 취미가 뭐에요?"
	show seah blush7 at seah_t1
	seah "아, 저는 주로 꽃을 보러 나가요."
	seah "방송실에 꽃이 있는 거 봤죠?"
	player "네. 선배님이 다 꾸며놓으신 건가요?"
	show seah happy1 at seah_t1
	seah "네! 제가 꽃으로 방 꾸미는 걸 좋아해서···."
	"아, 아까 방송실에 갔을 때 꽃이 여러 개 있는 이유가 그거였구나."
	show seah blush5 at seah_t1
	seah "그리고,, 그 중에 알스트로메리아란 꽃은······."
	"그 뒤로 목소리가 작아지는 바람에 선배의 말이 들리지 않는다."
	player "아! 알스트로메리아 꽃 되게 예쁘더라고요."
	"아까 여러모로 눈에 띄던 그 꽃의 이름이다."
	player "그나저나 아까 뭐라고 하셨어요? 잘 못 들어서···"
	show seah blush8 at seah_t1
	seah "아니에요오···."
	# 세아가 바로 옆에서 테이프 붙여주는 CG 등장
	show seah default at seah_t1
	"세아 선배는 테이프를 다시 힘껏 뜯어, 내 옆으로 천천히 다가왔다."
	"이번에는 테이프를 손에 건내지 않고 선배가 직접 붙여주었다."
	"선배가 테이프를 한땀한땀 붙이는 동안 나는 그저 가만히 있을 수 밖에 없었다.{p}만약 한 발짝 움직이기라도 했다면··· 세아 선배 어깨에 닿고 말았을 것이다."
	"세아 선배는 내가 이렇게 가까이 있다는 것조차 눈치 채지 못한 듯하다."
	"여전히 테이프 하나하나를 고심 끝에 붙이고 있었다."
	"잠시 후, 세아 선배는 테이프를 바닥에 놓고 포스터를 멀리서 보려고 뒷걸음질을 쳤다."
	show seah happy1 at seah_t1
	"선배는 붙인 포스터가 만족스러웠는지 표정에 환한 미소가 흘러넘치고 있었다."
	"세아 선배의 눈부신 웃음에 나도 왠지 모르게 뿌듯해진다."
	"이게 뭐라고. 고작 포스터 한 장일 뿐인데."
	show seah happy2 at seah_t1
	seah "이제 홈베이스에 다 붙였으니 계단 쪽 게시판으로 가볼까요?"
	player "네 선배님!"
