// Performance monitoring utility for collection updates
// Tracks and logs timing metrics to identify bottlenecks

interface PerformanceMetric {
  name: string;
  duration: number;
  timestamp: number;
}

class PerformanceMonitor {
  private metrics: PerformanceMetric[] = [];
  private marks = new Map<string, number>();
  private enabled = import.meta.env.DEV; // Only in development

  start(label: string) {
    if (!this.enabled) return;
    this.marks.set(label, performance.now());
  }

  end(label: string) {
    if (!this.enabled) return;
    const startTime = this.marks.get(label);
    if (startTime === undefined) {
      console.warn(`[PerformanceMonitor] No start mark for: ${label}`);
      return;
    }

    const duration = performance.now() - startTime;
    this.metrics.push({
      name: label,
      duration,
      timestamp: Date.now()
    });

    this.marks.delete(label);

    // Log slow operations
    if (duration > 10) {
      console.warn(`[PERF] ${label} took ${duration.toFixed(1)}ms`);
    }
  }

  measure<T>(label: string, fn: () => T): T {
    if (!this.enabled) {
      return fn();
    }

    const start = performance.now();
    const result = fn();
    const duration = performance.now() - start;

    this.metrics.push({
      name: label,
      duration,
      timestamp: Date.now()
    });

    if (duration > 10) {
      console.warn(`[PERF] ${label} took ${duration.toFixed(1)}ms`);
    }

    return result;
  }

  async measureAsync<T>(label: string, fn: () => Promise<T>): Promise<T> {
    if (!this.enabled) {
      return fn();
    }

    const start = performance.now();
    const result = await fn();
    const duration = performance.now() - start;

    this.metrics.push({
      name: label,
      duration,
      timestamp: Date.now()
    });

    if (duration > 50) {
      console.warn(`[PERF] ${label} took ${duration.toFixed(1)}ms`);
    }

    return result;
  }

  getMetrics() {
    return [...this.metrics];
  }

  clearMetrics() {
    this.metrics = [];
  }

  getSummary() {
    const summary = new Map<string, { count: number; total: number; avg: number; max: number }>();

    for (const metric of this.metrics) {
      const existing = summary.get(metric.name);
      if (existing) {
        existing.count++;
        existing.total += metric.duration;
        existing.avg = existing.total / existing.count;
        existing.max = Math.max(existing.max, metric.duration);
      } else {
        summary.set(metric.name, {
          count: 1,
          total: metric.duration,
          avg: metric.duration,
          max: metric.duration
        });
      }
    }

    return Object.fromEntries(summary);
  }

  logSummary() {
    if (!this.enabled) return;
    const summary = this.getSummary();
    console.table(summary);
  }
}

export const perfMonitor = new PerformanceMonitor();

interface WindowWithPerfMonitor extends Window {
  __perfMonitor?: PerformanceMonitor;
}

// Expose to window for debugging in development
if (import.meta.env.DEV) {
  (window as WindowWithPerfMonitor).__perfMonitor = perfMonitor;
  console.log('[PerformanceMonitor] Available at window.__perfMonitor');
  console.log('Commands: __perfMonitor.getSummary(), __perfMonitor.logSummary()');
}