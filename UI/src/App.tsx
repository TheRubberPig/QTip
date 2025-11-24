import "tailwindcss";

import { usePiiTracker } from './hooks/piiTracker';
import { SecureInput } from './components/SecureInput';

const Header = ({ stats }: { stats: number | null}) => (
  <header className="mb-8 flex justify-between items-center">
    <h1 className="text-3xl font-bold text-gray-800"> QTip </h1>
    <div className="bg-white px-6 py-4 rounded shadow font-medium text-gray-700 border border-gray-100">
        Total PII emails submitted: 
        <span className="ml-2 text-indigo-600 font-bold text-lg">
          {typeof stats === 'number' ? (stats) : (<span className="animate-pulse">...</span>)}
        </span>
    </div>
  </header>
);

function App() {
  // Hook into PII Tracker
  const { text, setText, stats, submit, loading } = usePiiTracker();

  return (
    <div className="min-h-screen bg-gray-100 p-8 font-sans text-gray-900">
      <div className="max-w-4xl mx-auto">
        
        <Header stats={stats} />

        <main className="bg-white rounded-lg shadow-lg p-6">
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Secure Input
          </label>
          
          <SecureInput value={text} onChange={setText} />
          
          <div className="flex justify-end mt-4">
            <button 
              onClick={submit} 
              disabled={loading}
              className={`
                font-bold py-2 px-6 rounded transition text-white
                ${loading ? 'bg-gray-400 cursor-not-allowed' : 'bg-indigo-600 hover:bg-indigo-700'}
              `}
            >
              {loading ? 'Processing...' : 'Submit'}
            </button>
          </div>
        </main>

      </div>
    </div>
  );
}

export default App