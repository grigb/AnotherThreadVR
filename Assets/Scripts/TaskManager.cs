﻿using UnityEngine;
using System.Collections.Generic;

namespace UTJ {

public class TaskManager {

	// singleton
	static TaskManager instance_;
	public static TaskManager Instance { get { return instance_ ?? (instance_ = new TaskManager()); } }

	private LinkedList<Task> task_list_;
	private LinkedList<Task> add_task_list_;
	private LinkedList<Task> del_task_list_;

	public void init()
	{
		task_list_ = new LinkedList<Task>();
		add_task_list_ = new LinkedList<Task>();
		del_task_list_ = new LinkedList<Task>();
	}

	public int getCount()
	{
		return task_list_.Count;
	}

	// Taskが呼ぶ
	public void add(Task task)
	{
		add_task_list_.AddLast(task); // 追加候補に追加 LinkedListNodeをnewしている(GCを汚している)
	}

	// Taskが呼ぶ
	public void remove(Task task)
	{
		del_task_list_.AddLast(task); // 削除候補に追加 LinkedListNodeをnewしている(GCを汚している)
	}

	public void restart()
	{
		for (var it = task_list_.First; it != null; it = it.Next) {
			it.Value.alive_ = false;
		}
		task_list_.Clear();
		add_task_list_.Clear();
		del_task_list_.Clear();
	}
	
	public void update(float dt, double update_time, float flow_speed)
	{
		for (var it = task_list_.First; it != null; it = it.Next) {
			it.Value.update(dt, update_time, flow_speed);
		}
		// add
		for (var it = add_task_list_.First; it != null; it = it.Next) {
			task_list_.AddLast(it.Value);
		}
		add_task_list_.Clear();
		// del
		for (var it = del_task_list_.First; it != null; it = it.Next) {
			task_list_.Remove(it.Value); // TODO 線形探索。重い！
		}
		del_task_list_.Clear();
	}

	public void renderUpdate(int front, MyCamera my_camera, ref DrawBuffer draw_buffer)
	{
		foreach (var task in task_list_) {
			task.renderUpdate(front, my_camera, ref draw_buffer);
		}
	}
}

} // namespace UTJ {
