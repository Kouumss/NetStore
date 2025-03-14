﻿namespace StoreNet.Domain.Layer.DTOs;

// Simple structure for API response
public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public bool Success { get; set; }
    public T Data { get; set; }
    public List<string> Errors { get; set; }

    // Default constructor
    public ApiResponse()
    {
        Success = true;
        Errors = new List<string>();
    }

    // Constructor for success with data
    public ApiResponse(int statusCode, T data)
    {
        StatusCode = statusCode;
        Success = true;
        Data = data;
        Errors = new List<string>();
    }

    // Constructor for failure with errors
    public ApiResponse(int statusCode, List<string> errors)
    {
        StatusCode = statusCode;
        Success = false;
        Errors = errors;
    }

    // Constructor for failure with a single error
    public ApiResponse(int statusCode, string error)
    {
        StatusCode = statusCode;
        Success = false;
        Errors = new List<string> { error };
    }
}

