using ElectronicObserver.Utility.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.ShipGroup
{


	[DataContract(Name = "ExpressionManager")]
	public sealed class ExpressionManager : DataStorage, ICloneable
	{

		private static string UILanguage = Utility.Configuration.Config.UI.Language;

		[DataMember]
		public List<ExpressionList> Expressions { get; set; }

		[IgnoreDataMember]
		private Expression<Func<ShipData, bool>> predicate;

		[IgnoreDataMember]
		private Expression expression;


		public ExpressionManager() : base()
		{
			Initialize();
		}

		public override void Initialize()
		{
			Expressions = new List<ExpressionList>();
			predicate = null;
			expression = null;
		}


		public ExpressionList this[int index]
		{
			get { return Expressions[index]; }
			set { Expressions[index] = value; }
		}


		public void Compile()
		{
			Expression ex = null;
			var paramex = Expression.Parameter(typeof(ShipData), "ship");

			foreach (var exlist in Expressions)
			{
				if (!exlist.Enabled)
					continue;

				if (ex == null)
				{
					ex = exlist.Compile(paramex);

				}
				else
				{
					if (exlist.ExternalAnd)
					{
						ex = Expression.AndAlso(ex, exlist.Compile(paramex));
					}
					else
					{
						ex = Expression.OrElse(ex, exlist.Compile(paramex));
					}
				}
			}


			if (ex == null)
			{
				ex = Expression.Constant(true, typeof(bool));       //:-P
			}

			predicate = Expression.Lambda<Func<ShipData, bool>>(ex, paramex);
			expression = ex;

		}


		public IEnumerable<ShipData> GetResult(IEnumerable<ShipData> list)
		{

			if (predicate == null) {
				switch (UILanguage) {
					case "zh":
						throw new InvalidOperationException("表达式未被编译。");
					case "en":
						throw new InvalidOperationException("Expressions aren't compiled.");
					default:
						throw new InvalidOperationException("式がコンパイルされていません。");
				}
			}

			return list.AsQueryable().Where(predicate).AsEnumerable();
		}

		public bool IsAvailable => predicate != null;



		public override string ToString()
		{

			if (Expressions == null)
			{
				switch (UILanguage) {
					case "zh":
						return "（无）";
					case "en":
						return "(Empty)";
					default:
						return "(なし)";
				}
			}

			StringBuilder sb = new StringBuilder();
			foreach (var ex in Expressions)
			{
				if (!ex.Enabled)
					continue;
				else if (sb.Length == 0)
					sb.Append(ex.ToString());
				else
				{
					switch (UILanguage) {
						case "zh":
							sb.Append($"{(ex.ExternalAnd ? "且" : "或")}{ex}");
							break;
						case "en":
							string ex_en = ex.ToString();
							if (ex_en.StartsWith("Doesn't"))
								ex_en = "doesn't" + ex_en.Substring(7);
							sb.Append($" {(ex.ExternalAnd ? "and" : "or")} {ex_en}");
							break;
						default:
							sb.Append($" {(ex.ExternalAnd ? "かつ" : "または")} {ex}");
							break;
					}
				}
			}

			if (sb.Length == 0)
			{
				switch (UILanguage) {
					case "zh":
						sb.Append("（无）");
						break;
					case "en":
						sb.Append("(Empty)");
						break;
					default:
						sb.Append("(なし)");
						break;
				}
			}
			return sb.ToString();
		}

		public string ToExpressionString()
		{
			return expression.ToString();
		}



		public ExpressionManager Clone()
		{
			var clone = (ExpressionManager)MemberwiseClone();
			clone.Expressions = Expressions?.Select(e => e.Clone()).ToList();
			clone.predicate = null;
			clone.expression = null;
			return clone;
		}

		object ICloneable.Clone()
		{
			return Clone();
		}


	}

}
