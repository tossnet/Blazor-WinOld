export function onLoad() {
    console.log('Loaded');
    hljs.highlightAll();
}

export function onUpdate() {
    console.log('Updated');
}

export function onDispose() {
    console.log('Disposed');
}