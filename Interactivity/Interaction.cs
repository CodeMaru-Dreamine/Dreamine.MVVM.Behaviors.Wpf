using Dreamine.MVVM.Behaviors.Core.Interfaces;
using System.ComponentModel;
using System.Windows;

namespace Dreamine.MVVM.Behaviors.Wpf.Interactivity
{
	/// <summary>
	/// 🧩 Dreamine용 Interaction static 헬퍼 클래스입니다.
	/// - WPF의 XAML에서 Behavior 컬렉션을 선언적으로 연결할 수 있게 도와줍니다.
	/// </summary>
	public static class Interaction
	{
		/// <summary>
		/// 'Behaviors'라는 이름으로 BehaviorCollection을 등록하는 DependencyProperty입니다.
		/// - 이 프로퍼티를 통해 XAML에서 Behavior 컬렉션을 연결합니다.
		/// </summary>
		public static readonly DependencyProperty BehaviorsProperty =
			DependencyProperty.RegisterAttached(
				"ShadowBehaviors", // XAML에서 사용될 프로퍼티 이름
				typeof(BehaviorCollection), // 이 프로퍼티가 가질 데이터 타입
				typeof(Interaction), // 프로퍼티가 등록될 타입 (Interaction)
				new FrameworkPropertyMetadata(
					new PropertyChangedCallback(OnBehaviorsChanged)) // 프로퍼티 변경 시 호출될 메서드
			);

		/// <summary>
		/// XAML에서 특정 DependencyObject에 연결된 Behavior 컬렉션을 가져옵니다.
		/// - BehaviorCollection이 없으면 새로 생성하고, 기본 Behavior를 추가합니다.
		/// </summary>
		/// <param name="obj">BehaviorCollection을 가져올 대상 객체</param>
		/// <returns>연결된 BehaviorCollection</returns>
		public static BehaviorCollection GetBehaviors(DependencyObject obj)
		{
			// 현재 BehaviorCollection을 가져옵니다.
			var collection = (BehaviorCollection)obj.GetValue(BehaviorsProperty);

			// 만약 BehaviorCollection이 없으면 새로 생성하고 기본 Behavior를 추가합니다.
			if (collection == null)
			{
				collection = new BehaviorCollection();

				// 예시로 WindowDragBehavior를 추가
				var behavior = new WindowDragBehavior();
				collection.Add(behavior); // Behavior를 컬렉션에 추가

				// 새로 생성한 컬렉션을 해당 객체에 연결
				SetBehaviors(obj, collection);
			}

			return collection; // BehaviorCollection을 반환
		}

		/// <summary>
		/// XAML에서 특정 DependencyObject에 BehaviorCollection을 설정합니다.
		/// </summary>
		/// <param name="obj">BehaviorCollection을 설정할 대상 객체</param>
		/// <param name="value">설정할 BehaviorCollection</param>
		public static void SetBehaviors(DependencyObject obj, BehaviorCollection value)
		{
			// XAML 객체에 BehaviorCollection을 설정
			obj.SetValue(BehaviorsProperty, value);
		}

        /// <summary>
        /// Behaviors 연결 속성이 변경될 때 호출됩니다.
        /// 이전 컬렉션은 대상 객체에서 분리하고,
        /// 새로운 컬렉션은 현재 대상 객체에 연결합니다.
        /// </summary>
        /// <param name="obj">BehaviorCollection이 연결될 대상 DependencyObject입니다.</param>
        /// <param name="args">변경 전/후 BehaviorCollection 정보를 포함하는 이벤트 인자입니다.</param>
        private static void OnBehaviorsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            // 변경 전/후 BehaviorCollection 참조를 가져옵니다.
            BehaviorCollection oldCollection = (BehaviorCollection)args.OldValue;
            BehaviorCollection newCollection = (BehaviorCollection)args.NewValue;

            // 동일한 컬렉션 참조라면 실제 변경이 없으므로 처리하지 않습니다.
            if (ReferenceEquals(oldCollection, newCollection))
            {
                return;
            }

            // 기존 컬렉션이 이미 어떤 대상 객체에 연결되어 있다면
            // 먼저 Detach 하여 이전 연결 상태를 정리합니다.
            if (oldCollection != null && ((IAttachedObject)oldCollection).AssociatedObject != null)
            {
                oldCollection.Detach();
            }

            // 새로운 컬렉션과 대상 객체가 모두 유효하면
            // 새로운 대상 객체에 BehaviorCollection을 연결합니다.
            if (newCollection != null && obj != null)
            {
                newCollection.Attach(obj);
            }
        }
    }
}
