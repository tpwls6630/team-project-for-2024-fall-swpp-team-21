# Computer Virus Survivors
3D로 구현한 뱀서라이크 게임입니다.
메모리 공간에서 끊임 없이 생성되는 바이러스들을 컴퓨터 백신이 되어 물리치는 컨셉의 게임입니다.  
![image](https://github.com/user-attachments/assets/fd27a1d3-cce8-493f-8e0d-c757040b6c89)

## 👨‍💻팀원
- 이*혁 (PM, Designer)
- 우세진 (Programmer)
- 전*언 (Programmer)
- 서*성 (Designer)

## 🛠️Tech Stack
- Unity 2021.3.42f1
- C#
- Blender
  
## ✨내가 구현한 핵심 기능 요약
- [8축 이동 기능](#8축-이동-기능)
- [레벨업 시 아이템 선택 기능](#레벨업-시-아이템-선택-기능)
- [Canvas UI를 FSM으로 구현](#canvas-ui-구현-fsm)
- [무기와 아이템 클래스의 추상화](#무기와-아이템-구현)
- [공격 이펙트(파티클 시스템으로 구현)](#공격-이펙트-파티클-시스템)
- [사운드 이펙트 + 사운드 조합 기능](#사운드-이펙트--사운드-조합-기능)  
이 외에도 다양한 부분을 구현 했지만 핵심이 되는 부분은 위와 같습니다

## 🎥Demo
WebGL 버전 배포판입니다. 
https://2024fall-swpp.github.io/team-project-for-2024-fall-swpp-team-21/

## 결과
 소프트웨어 개발 및 실습 게임 테스트 현장 투표에서 21개 조중 2위

## 프로젝트를 통해 배운 점
 게임을 만드는 프로젝트도, 여러 사람이 모여 프로그램을 개발하는 경험도 처음이었습니다. Slack을 통해 아이디어를 공유하고 일정을 조절했고, 구글 스프레드 시트를 통해 아이디어를 기록하고 스케줄을 기록했습니다. Github를 통해 오류를 기록하고 프로그램을 쌓아 나가 프로그램 개발 프로젝트가 '대략적으로' 어떻게 돌아가는지 체험하는 좋은 경험이었습니다. 그리고 8주밖에 되지 않는 기간동안 게임 개발 경험이 없는 4명의 팀원이 모여 이런 완성도의 게임을 만들 수 있었다는 것이 감개무량합니다.  
 본인이 떠올리고 있는 아이디어를 구체화하여 남에게 어필하여 설득한다는 것의 어려움을 깨달았습니다. 본인의 머릿 속에선 멋있는 게임이 돌아가고 있겠지만, 이를 말로 표현하여 요구사항을 전달한다는 것은 너무나 어려운 일이었습니다. 비슷한 게임, 비슷한 사운드, 비슷한 비주얼을 가진 예시를 들며 설명하는 것이 가장 효과적이었고 이를 위해선 평소에 다양한 문화생활을 경험하며 교양을 쌓을 필요가 있음을 느꼈습니다.

# Feature
## 8축 이동 기능
게임 개발 초기에 플레이어의 움직임을 구현하다 한 가지 버그를 발견하게 됩니다. 플레이어가 8가지 방향을 바라보도록 프로그래밍 하였는데, 대각선으로 이동하다 키보드를 떼는 경우 대각선을 바라보지 않고 대각선과 수직방향 사이의 어딘가를 바라보게 되었습니다.
이 문제의 원인을 "사용자는 키보드를 동시에 떼었다고 생각했지만 실제론 0.1초 내의 시간차이로 키보드가 차례로 떼어지는 상황"에 있다고 생각하였습니다. 

이 문제를 해결하기 위해 두 키가 떼어지는 시간차이가 일정 시간 이하일 경우 동시에 떼어지는 것으로 판정하기로 하였습니다.   
![output](https://github.com/user-attachments/assets/9404d101-5c32-46bd-ba5b-270ea9e31823)

👉[이동 구현](https://github.com/tpwls6630/team-project-for-2024-fall-swpp-team-21/blob/8fd1ced4609f994da35ea1e97737cd87167825ec/Computer%20Virus%20Survivors/Assets/Scripts/Player/PlayerController.cs#L54)  
```
private void Move()
    {
        ...
        // 수평 이동, 수직 이동 값을 불러옵니다
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // 해당 값의 절댓값의 비율로 방향(수평, 수직, 대각)을 결정합니다
        float horizontalAbs = Mathf.Abs(horizontalInput);
        float verticalAbs = Mathf.Abs(verticalInput);

        // 유니티의 인풋시스템의 Gravity는 키보드를 뗀 후 인풋값이 초당 얼마만큼 떨어지는 지를 정하는 값입니다
        // 이 프로젝트의 Gravity값은 12이므로 5프레임만에 인풋값이 0으로 되돌아갑니다(1프레임당 -0.2)
        // 수평방향 인풋과 수직방향 인풋의 값이 둘 다 0.6 이하라면 정지한 것으로 판단하는 부분입니다
        if (horizontalAbs < Cst.DeadZoneSec // Cst클래스는 게임의 주요 상수를 정적 필드로 저장하는 클래스입니다
            && verticalAbs < Cst.DeadZoneSec)
        {
            animator.SetBool("b_IsMoving", false);
            return;
        }

        // 8-axis movement
        if (horizontalAbs - verticalAbs > Cst.ThresholdSec)
        {
            // 수직 방향키를 뗀 후 3프레임 이상 지나면 수직 속력 0
            verticalInput = 0;
        }
        else if (verticalAbs - horizontalAbs > Cst.ThresholdSec)
        {
            // 수평 방향키를 뗀 후 3프레임 이상 지나면 수평 속력 0
            horizontalInput = 0;
        }
        else 
        {
            // 수평(혹은 수직) 방향키를 뗀 후 3프레임이 지나지 않았다면 대각선 방향
            // 수직방향 키(w,s)와 수평방향 키(a,d)를 3프레임 이내의 차이로 떼면 두 인풋값의 절댓값의 차이는 0.6 이상으로 차이나지 않게 됩니다
            // 각 축에 대해 가능한 속력을 {-1, 0, 1}로 제한하여 8축의 이산 방향 설정
            if (verticalAbs > .1f)
            {
                verticalInput /= verticalAbs;
            }

            if (horizontalAbs > .1f)
            {
                horizontalInput /= horizontalAbs;
            }
        }

        ...
    }
```


## 레벨업 시 아이템 선택 기능

레벨업 시 플레이어는 3가지 선택지 중 하나를 선택하여 얻게 됩니다. 각 선택지는 다음의 조건을 만족하는 무기 or 아이템 중 랜덤하게 3개가 결정됩니다.
1. 플레이어의 무기가 6종류가 아니라면 모든 무기종류가 나타남
2. 플레이어의 무기가 6종류라면 해당 무기종류만 나타남
3. 플레이어의 아이템이 6종류가 아니라면 모든 아이템종류가 나타남
4. 플레이어의 아이템이 6종류라면 해당 아이템종류만 나타남
5. 각 무기(아이템)을 최대레벨까지 강화한 경우 해당 무기(아이템)은 나타나지 않음

그리고 아이템을 선택하는 도중엔 시간이 멈추게 됩니다. 
한번에 레벨업을 여러번 하는 경우 레벨업 한 만큼 아이템을 선택하게 됩니다.

유니티 코루틴의 WaitUntil()기능을 모를땐 세마포어와 멀티 스레드를 이용해 아이템 선택이 끝날때까지 기다리는 await기능을 활용해 해결했었습니다. ([과거 구현](https://github.com/tpwls6630/team-project-for-2024-fall-swpp-team-21/commit/02be3eb19f725fe7af554dcd1f558950f0a2627e))  
그러나 WebGL로 배포하고 보니 레벨업 기능이 작동하지 않아 조사해보니 WebGL환경은 멀티 스레드를 지원하지 않아 생기는 버그였습니다.
결국 이를 해결하기 위해 코루틴 방식을 사용하게 되었습니다.

![image](https://github.com/user-attachments/assets/aa20a3e9-19f9-4d95-a587-8c02d021fd0f)


👉[레벨업 로직](
https://github.com/tpwls6630/team-project-for-2024-fall-swpp-team-21/blob/8fd1ced4609f994da35ea1e97737cd87167825ec/Computer%20Virus%20Survivors/Assets/Scripts/Player/PlayerStat.cs#L388)
```
private IEnumerator CurrentExpChanged()
    {
        while (currentExp >= maxExp)
        {
            PlayerLevel++; // Invoke Player Level Changed Event -> Show Item Selection Canvas

            // 아이템 선택이 완료됨을 이벤트 핸들러로 알아차리도록 하여 개방 폐쇄 원칙을 최대한 지키도록 하였습니다.
            ItemSelectCanvasManager.instance.SelectionHandler += ItemSelected;
            yield return new WaitUntil(() => itemSelected);
            ItemSelectCanvasManager.instance.SelectionHandler -= ItemSelected;

            currentExp -= maxExp;
            MaxExp = maxExpList[Mathf.Min(PlayerLevel, maxExpList.Length - 1)];
            itemSelected = false;
            statEventCaller.Invoke(nameof(CurrentExp), currentExp);
        }
        itemSelectingCoroutine = null;
    }

    public void ItemSelected(SelectableBehaviour seleted)
    {
        Debug.Log("Item Selected : " + seleted.ObjectName);
        TakeSelectable(seleted);
        itemSelected = true;
    }
```

👉[아이템 선택 캔버스](https://github.com/tpwls6630/team-project-for-2024-fall-swpp-team-21/blob/main/Computer%20Virus%20Survivors/Assets/Scripts/Canvas/ItemSelectCanvasManager.cs#L8 "코드를 확인하려면 클릭하세요")  
👉[아이템 선택 리스트 매니저](https://github.com/tpwls6630/team-project-for-2024-fall-swpp-team-21/blob/main/Computer%20Virus%20Survivors/Assets/Scripts/Selectable/SelectableManager.cs#L45 "코드를 확인하려면 클릭하세요")  

## Canvas UI 구현 (FSM)

게임 플레이의 State 관리를 UI 시스템이 담당하게 되었습니다. 논리상 State 관리는 따로 하고 캔버스는 해당 State에 맞게 띄워주는 것이 맞지만 각 State에 맞는 Canvas가 일대일 대응 관계이기 때문에 이렇게 구현해도 문제가 없었던 것 같습니다. 분명 FSM을 사용하긴 했는데 코드가 복잡한 것 같고 단일 책임 원칙에 위배된 것 같아 아쉬움이 남는 구현입니다.  
👉[Canvas 폴더](https://github.com/tpwls6630/team-project-for-2024-fall-swpp-team-21/tree/main/Computer%20Virus%20Survivors/Assets/Scripts/Canvas)  
👉[FSM](https://github.com/tpwls6630/team-project-for-2024-fall-swpp-team-21/blob/8fd1ced4609f994da35ea1e97737cd87167825ec/Computer%20Virus%20Survivors/Assets/Scripts/Canvas/CanvasManager.cs#L130)  
![image](https://github.com/user-attachments/assets/5f1a065a-7325-4acb-b80e-69bdb0110625)

## 무기와 아이템 구현

플레이어는 레벨업 할 때마다 무기와 아이템을 선택하여 얻을 수 있습니다. 때문에 [무기](https://github.com/tpwls6630/team-project-for-2024-fall-swpp-team-21/blob/8fd1ced4609f994da35ea1e97737cd87167825ec/Computer%20Virus%20Survivors/Assets/Scripts/Selectable/WeaponBehaviour.cs#L6)와 [아이템](https://github.com/tpwls6630/team-project-for-2024-fall-swpp-team-21/blob/8fd1ced4609f994da35ea1e97737cd87167825ec/Computer%20Virus%20Survivors/Assets/Scripts/Selectable/ItemBehaviour.cs#L1)은 [SelectableBahaviour](https://github.com/tpwls6630/team-project-for-2024-fall-swpp-team-21/blob/8fd1ced4609f994da35ea1e97737cd87167825ec/Computer%20Virus%20Survivors/Assets/Scripts/Selectable/SelectableBehaviour.cs#L6)라는 추상 클래스를 상속합니다.
덕분에 아이템 선택 창에서 SelectableBahaviour클래스를 이용해 쉽게 무기 & 아이템 후보군을 뽑아낼 수 있습니다. 
또 새로운 무기와 아이템 컨텐츠를 추가하기 쉬운 환경을 만들었습니다. 
👉[Selectable 폴더](https://github.com/tpwls6630/team-project-for-2024-fall-swpp-team-21/tree/main/Computer%20Virus%20Survivors/Assets/Scripts/Selectable)
![image](https://github.com/user-attachments/assets/5c83ccb2-a34e-41bc-b9b8-3a282e18c9fb)

## 공격 이펙트 (파티클 시스템)

무료 이펙트 에셋 ([링크](https://assetstore.unity.com/packages/vfx/particles/hit-effects-free-284613))를 손봐서 사용했습니다. 
무기에서 발사된 투사체가 적에게 맞을 때 파티클 시스템을 해당 위치에 생성하는 식으로 구현하였습니다. 투사체에 파티클 시스템을 포함할 수 있었지만 그렇게 구현하면 투사체의 단일 책임 원칙에 위배된다고 생각했습니다. 이펙트가 따로 존재해야 프리팹의 재사용성이 높다고 생각하기도 했습니다. 

크리티컬 공격이 발생하면 크리티컬 이펙트가 생성됩니다.
무기별 투사체는 공격 이펙트 타입을 설정할 수 있습니다.
![image](https://github.com/user-attachments/assets/2aefaf1b-0954-41b9-8bba-21f4c312c38a)

```
public enum AttackEffectType
{
    Basic,
    Basic2,
    Basic3,
    Explosion,
    Ice,
    Lightning,
    Magic,

}
```

투사체의 공격 이펙트 호출은 [투사체 추상 클래스](https://github.com/tpwls6630/team-project-for-2024-fall-swpp-team-21/blob/8fd1ced4609f994da35ea1e97737cd87167825ec/Computer%20Virus%20Survivors/Assets/Scripts/Selectable/ProjectileBehaviour.cs#L4)에서 정의됩니다. 각 투사체의 공격 판정 시 이 함수를 호출하는 것으로 간단히 구현됩니다.
```
public abstract class ProjectileBehaviour : MonoBehaviour
{
    protected abstract void OnTriggerEnter(Collider other);
    private AttackEffect attackEffect = new AttackEffect(); // AttackEffect.Play를 정적 메소드로 만든다면 필요 없는 부분. 리소스 낭비.
    [SerializeField] private AttackEffectType attackEffectType = AttackEffectType.Basic;

    protected virtual void PlayAttackEffect(Vector3 hitPosition, Quaternion rotation, bool isCritical = false, float scale = 1.0f)
    {
        attackEffect.Play(hitPosition, rotation, attackEffectType, isCritical, scale);
    }

    ...
}
```

지금 다시 리팩토링 한다면 AttackEffect 클래스의 메소드들을 정적 메소드로 바꾸어 투사체 추상 클래스에서 AttackEffect클래스를 개체화 할 필요 없이 바로 AttackEffect.Play를 호출하도록 할 것입니다. 

## 사운드 이펙트 + 사운드 조합 기능

저희 팀엔 사운드 담당 아티스트가 없었습니다. 그래서 무료 사운드 에셋을 짜집기하여 다채로운 사운드를 구현하고자 하는 노력이 있었습니다. 
핵심 아이디어는 다음과 같습니다.  

- 사운드 클립을 특정 구간만 플레이 함(한번 혹은 n번 반복)
- 이 사운드 클립을 여러 개 이어 붙힘

이 기능으로 포탑이 공격을 차징하는 소리, 트로이 목마 중간 보스의 돌진 소리를 구현하였습니다.  

👉[사운드 프리셋](https://github.com/tpwls6630/team-project-for-2024-fall-swpp-team-21/blob/main/Computer%20Virus%20Survivors/Assets/Scripts/SFXSequencePreset.cs)
👉[사운드 매니저](https://github.com/tpwls6630/team-project-for-2024-fall-swpp-team-21/blob/main/Computer%20Virus%20Survivors/Assets/Scripts/SFXManager.cs)
  
### <포탑 공격 차지>  
continous_beam_4 사운드를 0초부터 1초까지 실행하고  
continous_beam_4 사운드를 1초부터 3초 구간을 10번 반복하는 사운드입니다  
![image](https://github.com/user-attachments/assets/8036db62-a8b0-4cda-b25a-b222d4e307fc)

### <포탑 공격 임박>  
Menu_Navigate_00 사운드를 0.03초부터 0.13초 구간(0.1초)를 10번 반복하여 깜빡거리는 소리를 구현했습니다.  
![image](https://github.com/user-attachments/assets/2d81d129-5357-41f0-9547-0cde26caecc5)

### <트로이 목마 대시>  
서로 다른 두 사운드를 조합하여 새로운 사운드 클립을 만들어낸 예시입니다  
![image](https://github.com/user-attachments/assets/807dfb94-186e-445a-a6c1-287e9c29059f)

