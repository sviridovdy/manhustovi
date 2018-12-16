using System.Collections.Generic;
using System.Diagnostics;

namespace manhustovi.admin.Services
{
	public class ProcessingEvents
	{
		private readonly List<string> _events;

		private readonly Stopwatch _stopwatch;

		private bool _isComplete;

		public ProcessingEvents()
		{
			_events = new List<string>();
			_stopwatch = Stopwatch.StartNew();
		}

		public void AddEvent(string message)
		{
			if (_isComplete)
			{
				return;
			}

			lock (_events)
			{
				_events.Add($"T+{(int) _stopwatch.Elapsed.TotalSeconds}: {message}");
			}
		}

		public string[] DrainEvents()
		{
			lock (_events)
			{
				if (_isComplete && _events.Count == 0)
				{
					return null;
				}

				var events = _events.ToArray();
				_events.Clear();
				return events;
			}
		}

		public void Complete()
		{
			_isComplete = true;
		}
	}
}