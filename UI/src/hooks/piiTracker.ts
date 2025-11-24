import { useState, useEffect } from 'react';
import { ApiService } from '../services/qtipApi';

export const usePiiTracker = () => {
  const [text, setText] = useState('');
  const [stats, setStats] = useState<number | null>(null);
  const [loading, setLoading] = useState(false);

  const refreshStats = async () => {
    try {
      const data = await ApiService.getStats();
      setStats(data.emailCount);
    } catch (error) {
      console.error("Error fetching stats:", error);
    }
  };

  // Initial Load
  useEffect(() => {
    refreshStats();
  }, []);

  const submit = async () => {
    if (!text) return;
    setLoading(true);
    try {
      await ApiService.submitText(text);
      setText('');
      await refreshStats();
    } catch (error) {
      console.error("Error submitting:", error);
    } finally {
      setLoading(false);
    }
  };

  return {
    text,
    setText,
    stats,
    submit,
    loading
  };
};