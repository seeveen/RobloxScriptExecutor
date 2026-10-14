-- Roblox Studio Plugin: Script Executor Bridge
-- This plugin polls a local server for scripts to execute.

local HttpService = game:GetService("HttpService")
local LogService = game:GetService("LogService")

local SERVER_URL = "http://localhost:8080/poll"
local POLL_INTERVAL = 2.0 -- Seconds between polls

local function executeScript(scriptContent, scriptName)
	print("[Executor] Executing script: " .. tostring(scriptName))

	local success, err = pcall(function()
		-- In a real Studio environment, loadstring is required.
		-- Ensure "Allow HTTP Requests" and "Enable LoadString" are enabled in Studio settings.
		local func = loadstring(scriptContent)
		if func then
			func()
		else
			error("Failed to parse script content")
		end
	end)

	if not success then
		warn("[Executor] Error executing " .. tostring(scriptName) .. ": " .. tostring(err))
	end
end

local function pollServer()
	local success, response = pcall(function()
		return HttpService:GetAsync(SERVER_URL)
	end)

	if success then
		local data = HttpService:JSONDecode(response)
		if data.HasNewScript then
			executeScript(data.ScriptContent, data.ScriptName)
		end
	else
		-- Quietly fail to avoid spamming the output with HTTP errors
		-- warn("[Executor] Server offline or unreachable")
	end
end

-- Main Loop
print("[Executor] Bridge Plugin Loaded. Listening for scripts...")
while true do
	pollServer()
	task.wait(POLL_INTERVAL)
end
